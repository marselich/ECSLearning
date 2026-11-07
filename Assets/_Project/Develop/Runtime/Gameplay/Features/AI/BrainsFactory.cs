using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private const float MINIMUM_TARGET_ENERGY_FRACTION = 0.4f;

        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly AIBrainsContext _brainsContext;
        private readonly IInputService _inputService;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public BrainsFactory(DIContainer container)
        {
            _container = container;
            _timerServiceFactory = container.Resolve<TimerServiceFactory>();
            _brainsContext = container.Resolve<AIBrainsContext>();
            _inputService = container.Resolve<IInputService>();
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
        }

        public StateMachineBrain CreateMainCharacterBrain(Entity entity)
        {
            MovingState moving = new MovingState(entity, _inputService);
            AimingState aiming = new AimingState(entity, _inputService);
            ShootingState shooting = new ShootingState(entity);

            ICompositeCondition shouldMove = new CompositeCondition()
                .Add(entity.CanMove)
                .Add(new FuncCondition(() => _inputService.Direction.sqrMagnitude > 0));

            ICompositeCondition shouldStop = new CompositeCondition()
                .Add(new FuncCondition(() => shouldMove.Evaluate() == false));

            ICompositeCondition attackEnded = new CompositeCondition()
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false));

            ICompositeCondition shouldShoot = new CompositeCondition()
                .Add(shouldStop)
                .Add(entity.CanMove)
                .Add(attackEnded)
                .Add(new FuncCondition(() => _inputService.AttackPressed));

            AIStateMachine movement = new AIStateMachine();

            movement.AddState(moving);

            AIStateMachine combat = new AIStateMachine(resetOnExit: true);

            combat.AddState(aiming);
            combat.AddState(shooting);

            combat.AddTransition(aiming, shooting, shouldShoot);
            combat.AddTransition(shooting, shooting, shouldShoot);
            combat.AddTransition(shooting, aiming, attackEnded);

            AIStateMachine rootStateMachine = new AIStateMachine();

            rootStateMachine.AddState(combat);
            rootStateMachine.AddState(movement);

            rootStateMachine.AddTransition(combat, movement, shouldMove);
            rootStateMachine.AddTransition(movement, combat, shouldStop);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateTeleportingCharacterBrain(Entity entity, ITargetSelector targetSelector)
        {
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition randomMode = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value == null));

            ICompositeCondition targetMode = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null));

            ICompositeCondition hasTargetTeleportEnergy = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentEnergy.Value >=
                    entity.MaxEnergy.Value * MINIMUM_TARGET_ENERGY_FRACTION));

            AIStateMachine randomTeleporting = CreateTeleportingBehaviour(
                entity,
                new RandomTeleportingState(entity),
                entity.TeleportInterval,
                randomMode);

            AIStateMachine targetTeleporting = CreateTeleportingBehaviour(
                entity,
                new TargetTeleportingState(entity),
                entity.TargetTeleportInterval,
                targetMode,
                hasTargetTeleportEnergy);

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);

            AIParallelState parallelState = new AIParallelState(findTargetState, randomTeleporting, targetTeleporting);

            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateTeleportingBehaviour(
            Entity entity,
            EmptyState teleportingState,
            ReactiveVariable<float> interval,
            ICompositeCondition isActiveMode,
            ICompositeCondition additionalCondition = null)
        {
            TeleportWaitingState waitingState = new TeleportWaitingState(interval, isActiveMode);

            ICompositeCondition isNotTeleporting = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsTeleporting.Value == false));

            ICompositeCondition canStartTeleport = new CompositeCondition()
                .Add(new FuncCondition(() => waitingState.IsReady))
                .Add(new FuncCondition(() => entity.IsInit))
                .Add(isNotTeleporting)
                .Add(new FuncCondition(() => entity.TeleportingRadius.Value > 0))
                .Add(entity.CanTeleport);

            if (additionalCondition != null)
                canStartTeleport.Add(additionalCondition);

            AIStateMachine behaviour = new AIStateMachine();

            behaviour.AddState(waitingState);
            behaviour.AddState(teleportingState);

            behaviour.AddTransition(waitingState, teleportingState, canStartTeleport);
            behaviour.AddTransition(teleportingState, waitingState, isNotTeleporting);

            return behaviour;
        }
    }
}
