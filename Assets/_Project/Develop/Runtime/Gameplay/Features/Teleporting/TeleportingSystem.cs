using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleporting
{
    public class TeleportingSystem : IInitializableSystem, IUpdatableSystem, IDisposable
    {
        private ReactiveVariable<float> _radius;
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<bool> _isTeleporting;
        private Rigidbody _rigidbody;

        private IDisposable _isTeleportingDisposable;

        public void OnInit(Entity entity)
        {
            _radius = entity.TeleportingRadius;
            _currentTime = entity.TeleportProcessCurrentTime;
            _initialTime = entity.TeleportProcessInitialTime;
            _isTeleporting = entity.IsTeleporting;
            _rigidbody = entity.Rigidbody;

            _isTeleportingDisposable = _isTeleporting.Subscribe(OnTeleporting);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isTeleporting.Value)
                return;

            _currentTime.Value += deltaTime;

            if (CooldownIsOver())
                _isTeleporting.Value = true;
        }

        public void Dispose()
        {
            _isTeleportingDisposable.Dispose();
        }

        private void OnTeleporting(bool arg1, bool isTeleporting)
        {
            if (isTeleporting)
            {
                Teleport();
                _isTeleporting.Value = false;
                _currentTime.Value = 0;
            }
        }

        private void Teleport()
        {
            Vector2 direction = Random.insideUnitCircle.normalized * _radius.Value;

            Vector3 targetPosition = _rigidbody.position + new Vector3(direction.x, _rigidbody.position.y, direction.y);

            _rigidbody.position = targetPosition;
        }

        private bool CooldownIsOver() => _currentTime.Value >= _initialTime.Value;
    }
}