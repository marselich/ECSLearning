using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TakeDamage
{
    public class DealAoEDamageAfterTeleportingSystem : IInitializableSystem, IDisposable
    {
        private Buffer<Entity> _contacts;
        private ReactiveVariable<bool> _isTeleporting;
        private ReactiveVariable<float> _damage;

        private List<Entity> _processedEntities;
        private IDisposable _isTeleportingDisposable;

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactEntitiesBuffer;
            _isTeleporting = entity.IsTeleporting;
            _damage = entity.AreaOfEffectDamage;

            _processedEntities = new List<Entity>(_contacts.Items.Length);

            _isTeleportingDisposable = _isTeleporting.Subscribe(OnTeleporting);
        }

        public void OnTeleporting(bool arg1, bool isTeleporting)
        {
            if (isTeleporting == false)
                return;

            for (int i = 0; i < _contacts.Count; i++)
            {
                Entity contactEntity = _contacts.Items[i];


                if (contactEntity.HasComponent<TakeDamageRequest>())
                {
                    contactEntity.TakeDamageRequest.Invoke(_damage.Value);

                    Debug.Log("Урон нанесен: " + _damage.Value);
                }
            }
        }

        public void Dispose()
        {
            _isTeleportingDisposable.Dispose();
        }
    }
}