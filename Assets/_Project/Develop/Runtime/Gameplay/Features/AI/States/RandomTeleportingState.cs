using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomTeleportingState : EmptyState
    {
        private readonly Entity _entity;

        public RandomTeleportingState(Entity entity)
        {
            _entity = entity;
        }

        public override void Enter()
        {
            base.Enter();

            Vector3 position = _entity.Rigidbody.position;
            Vector2 offset = Random.insideUnitCircle * _entity.TeleportingRadius.Value;
            _entity.TeleportDestination.Value = position + new Vector3(offset.x, 0, offset.y);
            _entity.IsTeleporting.Value = true;
        }
    }
}