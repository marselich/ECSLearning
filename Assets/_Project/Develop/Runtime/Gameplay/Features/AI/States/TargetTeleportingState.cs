using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class TargetTeleportingState : EmptyState
    {
        private readonly Entity _entity;

        public TargetTeleportingState(Entity entity)
        {
            _entity = entity;
        }

        public override void Enter()
        {
            base.Enter();

            Vector3 position = _entity.Rigidbody.position;
            Vector3 offset = _entity.CurrentTarget.Value.Transform.position - position;
            offset.y = 0;

            _entity.TeleportDestination.Value = position + Vector3.ClampMagnitude(offset, _entity.TeleportingRadius.Value);
            _entity.IsTeleporting.Value = true;
        }
    }
}