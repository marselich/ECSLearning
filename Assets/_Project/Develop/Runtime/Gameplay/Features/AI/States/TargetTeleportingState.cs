using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class TargetTeleportingState : State, IUpdatableState
    {
        private const float MinimumEnergyPercent = 40f;

        private readonly Entity _entity;
        private readonly ICompositeCondition _canTeleport;
        private readonly ReactiveVariable<float> _currentEnergy;
        private readonly ReactiveVariable<float> _currentHealth;
        private readonly ReactiveVariable<Entity> _currentTarget;
        private readonly ReactiveVariable<bool> _isDead;
        private readonly ReactiveVariable<bool> _isTeleporting;
        private readonly ReactiveVariable<float> _maxEnergy;
        private readonly Rigidbody _rigidbody;
        private readonly ReactiveVariable<Vector3> _teleportDestination;
        private readonly ReactiveVariable<float> _teleportingRadius;
        private readonly ReactiveVariable<float> _targetTeleportInterval;
        private readonly ReactiveVariable<float> _elapsedTime = new();

        public TargetTeleportingState(Entity entity)
        {
            _entity = entity;
            _canTeleport = entity.CanTeleport;
            _currentEnergy = entity.CurrentEnergy;
            _currentHealth = entity.CurrentHealth;
            _currentTarget = entity.CurrentTarget;
            _isDead = entity.IsDead;
            _isTeleporting = entity.IsTeleporting;
            _maxEnergy = entity.MaxEnergy;
            _rigidbody = entity.Rigidbody;
            _teleportDestination = entity.TeleportDestination;
            _teleportingRadius = entity.TeleportingRadius;
            _targetTeleportInterval = entity.TargetTeleportInterval;
        }

        public override void Enter()
        {
            _elapsedTime.Value = 0;
            base.Enter();
        }

        public void Update(float deltaTime)
        {
            if (_entity.IsInit == false || _isDead.Value || _currentHealth.Value <= 0)
                return;

            _elapsedTime.Value = Mathf.Min(_elapsedTime.Value + deltaTime, _targetTeleportInterval.Value);

            if (_elapsedTime.Value < _targetTeleportInterval.Value || _isTeleporting.Value
                || _canTeleport.Evaluate() == false || _teleportingRadius.Value <= 0)
                return;

            if (_currentEnergy.Value < _maxEnergy.Value * MinimumEnergyPercent / 100f)
                return;

            Entity target = _currentTarget.Value;

            if (target == null)
                return;

            Vector3 offset = target.Transform.position - _rigidbody.position;
            offset.y = 0;

            _teleportDestination.Value = _rigidbody.position + Vector3.ClampMagnitude(offset, _teleportingRadius.Value);
            _isTeleporting.Value = true;
            _elapsedTime.Value = 0;
        }
    }
}
