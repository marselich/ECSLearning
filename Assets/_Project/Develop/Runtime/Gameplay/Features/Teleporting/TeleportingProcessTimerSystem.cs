using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleporting
{
    public class TeleportingProcessTimerSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<bool> _isTeleporting;

        private IDisposable _isTeleportingDisposable;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.TeleportProcessCurrentTime;
            _initialTime = entity.TeleportProcessInitialTime;
            _isTeleporting = entity.IsTeleporting;

            _isTeleportingDisposable = _isTeleporting.Subscribe(OnTeleporting);
        }

        public void Dispose()
        {
            _isTeleportingDisposable.Dispose();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isTeleporting.Value)
                return;

            _currentTime.Value += deltaTime;

            if (CooldownIsOver())
                _isTeleporting.Value = true;
        }

        private void OnTeleporting(bool arg1, bool isTeleporting)
        {
            if (isTeleporting)
                _currentTime.Value = 0;
        }

        private bool CooldownIsOver() => _currentTime.Value >= _initialTime.Value;
    }
}