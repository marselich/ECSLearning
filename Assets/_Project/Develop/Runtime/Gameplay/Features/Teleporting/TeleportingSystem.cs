using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleporting
{
    public class TeleportingSystem : IInitializableSystem, IDisposable
    {
        private ReactiveVariable<Vector3> _destination;
        private ReactiveVariable<bool> _isTeleporting;
        private ICompositeCondition _canTeleport;
        private Rigidbody _rigidbody;

        private IDisposable _isTeleportingDisposable;

        public void OnInit(Entity entity)
        {
            _destination = entity.TeleportDestination;
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
            if (isTeleporting == false)
                return;

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
            _rigidbody.position = _destination.Value;
        }
    }
}
