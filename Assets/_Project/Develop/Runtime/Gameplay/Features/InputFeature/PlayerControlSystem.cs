using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class PlayerControlSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly IInputService _input;
        private ReactiveVariable<bool> _isDead;
        private ReactiveVariable<float> _currentHealth;
        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<Vector3> _rotationDirection;
        private ReactiveVariable<bool> _inAttackProcess;
        private ReactiveEvent _startAttackRequest;
        private Transform _transform;
        private ReactiveVariable<float> _mouseSensitivity;

        public PlayerControlSystem(IInputService input) => _input = input;

        public void OnInit(Entity entity)
        {
            _isDead = entity.IsDead;
            _currentHealth = entity.CurrentHealth;
            _moveDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
            _inAttackProcess = entity.InAttackProcess;
            _startAttackRequest = entity.StartAttackRequest;
            _transform = entity.Transform;
            _mouseSensitivity = entity.MouseSensitivity;
        }

        public void OnUpdate(float deltaTime)
        {
            bool alive = !_isDead.Value && _currentHealth.Value > 0;
            Vector3 direction = alive ? _input.Direction : Vector3.zero;
            _moveDirection.Value = direction;

            if (direction.sqrMagnitude > 0)
                _rotationDirection.Value = direction;
            else if (alive && !_inAttackProcess.Value)
            {
                _rotationDirection.Value = Quaternion.AngleAxis(
                    _input.MouseHorizontal * _mouseSensitivity.Value, Vector3.up) * _transform.forward;
            }

            if (alive && direction.sqrMagnitude == 0 && _input.AttackPressed)
                _startAttackRequest.Invoke();
        }
    }
}
