using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleporting
{
    public class TeleportingSystem : IInitializableSystem, IDisposable
    {
        private ReactiveVariable<float> _radius;
        private ReactiveVariable<bool> _isTeleporting;
        private ICompositeCondition _canTeleport;
        private Rigidbody _rigidbody;

        private IDisposable _isTeleportingDisposable;

        public void OnInit(Entity entity)
        {
            _radius = entity.TeleportingRadius;
            _isTeleporting = entity.IsTeleporting;
            _rigidbody = entity.Rigidbody;
            _canTeleport = entity.CanTeleport;

            _isTeleportingDisposable = _isTeleporting.Subscribe(OnTeleporting);
        }

        public void Dispose()
        {
            _isTeleportingDisposable.Dispose();
        }

        private void OnTeleporting(bool arg1, bool isTeleporting)
        {
            if (_canTeleport.Evaluate() == false)
            {
                _isTeleporting.Value = false;
                return;
            }

            if (isTeleporting && _canTeleport.Evaluate())
            {
                Teleport();
                _isTeleporting.Value = false;
            }
        }

        private void Teleport()
        {
            Vector2 direction = Random.insideUnitCircle.normalized * _radius.Value;

            Vector3 targetPosition = _rigidbody.position + new Vector3(direction.x, _rigidbody.position.y, direction.y);

            _rigidbody.position = targetPosition;
        }
    }
}