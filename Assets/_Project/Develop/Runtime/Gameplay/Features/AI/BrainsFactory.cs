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

        public StateMachineBrain CreateTeleportingCharacterBrain(Entity entity, ITargetSelector targetSelector)
        {
            RandomTeleportingState randomState = new RandomTeleportingState(entity);
            TargetTeleportingState targetState = new TargetTeleportingState(entity);
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromRandomToTarget = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null));

            ICompositeCondition fromTargetToRandom = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value == null));

            AIStateMachine behaviour = new AIStateMachine();
            behaviour.AddState(randomState);
            behaviour.AddState(targetState);
            behaviour.AddTransition(randomState, targetState, fromRandomToTarget);
            behaviour.AddTransition(targetState, randomState, fromTargetToRandom);

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
            AIParallelState parallelState = new AIParallelState(findTargetState, behaviour);
            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
            _brainsContext.SetFor(entity, brain);
            return brain;
        }
    }
}
