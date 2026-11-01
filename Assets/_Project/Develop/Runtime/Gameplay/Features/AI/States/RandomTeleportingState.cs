using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomTeleportingState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly ICompositeCondition _canTeleport;
        private readonly ReactiveVariable<float> _currentHealth;
        private readonly ReactiveVariable<bool> _isDead;
        private readonly ReactiveVariable<bool> _isTeleporting;
        private readonly Rigidbody _rigidbody;
        private readonly ReactiveVariable<Vector3> _teleportDestination;
        private readonly ReactiveVariable<float> _teleportingRadius;
        private readonly ReactiveVariable<float> _teleportInterval;
        private readonly ReactiveVariable<float> _elapsedTime = new();

        public RandomTeleportingState(Entity entity)
        {
            _entity = entity;
            _canTeleport = entity.CanTeleport;
            _currentHealth = entity.CurrentHealth;
            _isDead = entity.IsDead;
            _isTeleporting = entity.IsTeleporting;
            _rigidbody = entity.Rigidbody;
            _teleportDestination = entity.TeleportDestination;
            _teleportingRadius = entity.TeleportingRadius;
            _teleportInterval = entity.TeleportInterval;
        }

        public override void Enter()
        {
            base.Enter();

            _elapsedTime.Value = 0;
        }

        public void Update(float deltaTime)
        {
            if (_entity.IsInit == false || _isDead.Value || _currentHealth.Value <= 0)
                return;

            _elapsedTime.Value = Mathf.Min(_elapsedTime.Value + deltaTime, _teleportInterval.Value);

            if (_elapsedTime.Value < _teleportInterval.Value || _isTeleporting.Value
                || _canTeleport.Evaluate() == false || _teleportingRadius.Value <= 0)
                return;

            Vector2 offset = Random.insideUnitCircle * _teleportingRadius.Value;

            _teleportDestination.Value = _rigidbody.position + new Vector3(offset.x, 0, offset.y);

            _isTeleporting.Value = true;

            _elapsedTime.Value = 0;
        }
    }
}