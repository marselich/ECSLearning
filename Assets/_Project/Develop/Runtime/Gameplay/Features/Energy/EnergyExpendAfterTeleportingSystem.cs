using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class EnergyExpendAfterTeleportingSystem : IInitializableSystem, IDisposable
    {
        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _energyExpend;
        private ReactiveVariable<bool> _isTeleporting;
        private ICompositeCondition _canTeleport;

        private IDisposable _isTeleportingDisposable;

        public void OnInit(Entity entity)
        {
            _currentEnergy = entity.CurrentEnergy;
            _energyExpend = entity.EnergyExpend;
            _isTeleporting = entity.IsTeleporting;
            _canTeleport = entity.CanTeleport;

            _isTeleportingDisposable = _isTeleporting.Subscribe(OnTeleporting);
        }

        public void Dispose()
        {
            _isTeleportingDisposable.Dispose();
        }

        private void OnTeleporting(bool arg1, bool isTeleporting)
        {
            if (isTeleporting && _canTeleport.Evaluate())
            {
                _currentEnergy.Value = MathF.Max(_currentEnergy.Value - _energyExpend.Value, 0);
                Debug.Log($"Энергия затрачена! Энергии: {_currentEnergy.Value}");
            }
        }
    }
}