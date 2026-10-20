using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class EnergyRestorationTimerSystem : IInitializableSystem, IUpdatableSystem, IDisposable
    {
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<bool> _isEnergyRestore;

        private IDisposable _isEnergyRestoreDisposable;

        public void OnInit(Entity entity)
        {
            _initialTime = entity.EnergyRestorationInitialTime;
            _currentTime = entity.EnergyRestorationCurrentTime;
            _isEnergyRestore = entity.IsEnergyRestore;

            _isEnergyRestoreDisposable = _isEnergyRestore.Subscribe(OnEnergyRestore);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isEnergyRestore.Value)
                return;

            _currentTime.Value += deltaTime;

            if (CooldownIsOver())
                _isEnergyRestore.Value = true;
        }

        public void Dispose()
        {
            _isEnergyRestoreDisposable.Dispose();
        }

        private void OnEnergyRestore(bool arg1, bool isEnergyRestore)
        {
            if (isEnergyRestore)
            {
                _currentTime.Value = 0;
                _isEnergyRestore.Value = false;
            }
        }

        private bool CooldownIsOver() => _currentTime.Value >= _initialTime.Value;
    }
}