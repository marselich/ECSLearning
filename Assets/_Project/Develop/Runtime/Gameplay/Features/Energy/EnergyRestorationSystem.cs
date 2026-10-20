using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class EnergyRestorationSystem : IInitializableSystem, IDisposable
    {
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _energyRestorationPercent;
        private ReactiveVariable<bool> _isEnergyRestore;

        private IDisposable _isEnergyRestoreDisposable;

        public void OnInit(Entity entity)
        {
            _maxEnergy = entity.MaxEnergy;
            _currentEnergy = entity.CurrentEnergy;
            _energyRestorationPercent = entity.EnergyRestorationPercent;
            _isEnergyRestore = entity.IsEnergyRestore;

            _isEnergyRestoreDisposable = _isEnergyRestore.Subscribe(OnEnergyRestore);
        }

        private void OnEnergyRestore(bool arg1, bool isEnergyRestore)
        {
            if (isEnergyRestore)
            {
                _currentEnergy.Value = MathF.Min(_currentEnergy.Value + GetPercentOfMaxEnergy(), _maxEnergy.Value);

                Debug.Log($"Энергия восстановлена: {_currentEnergy.Value}/{_maxEnergy.Value} энергии");
            }
        }

        public void Dispose()
        {
            _isEnergyRestoreDisposable.Dispose();
        }

        private float GetPercentOfMaxEnergy()
            => _maxEnergy.Value / 100 * _energyRestorationPercent.Value;
    }
}