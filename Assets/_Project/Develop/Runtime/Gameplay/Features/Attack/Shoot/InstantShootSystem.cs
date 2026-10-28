using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class InstantShootSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly EntitiesFactory _entitiesFactory;
        private ReactiveVariable<bool> _inAttackProcess;
        private ReactiveVariable<bool> _isDead;
        private ReactiveVariable<float> _currentHealth;
        private ReactiveVariable<float> _damage;
        private Transform _shootPoint;
        private IDisposable _subscription;

        public InstantShootSystem(EntitiesFactory entitiesFactory) => _entitiesFactory = entitiesFactory;

        public void OnInit(Entity entity)
        {
            _inAttackProcess = entity.InAttackProcess;
            _isDead = entity.IsDead;
            _currentHealth = entity.CurrentHealth;
            _damage = entity.InstantAttackDamage;
            _shootPoint = entity.ShootPoint;
            _subscription = entity.AttackDelayEndEvent.Subscribe(Shoot);
        }

        private void Shoot()
        {
            if (!_inAttackProcess.Value || _isDead.Value || _currentHealth.Value <= 0)
                return;

            _entitiesFactory.CreateProjectile(_shootPoint.position, _shootPoint.forward, _damage.Value);
        }

        public void OnDispose() => _subscription.Dispose();
    }
}
