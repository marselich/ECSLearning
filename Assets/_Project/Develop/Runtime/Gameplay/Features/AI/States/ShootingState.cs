using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    internal class ShootingState : EmptyState
    {
        private readonly Entity _entity;

        public ShootingState(Entity entity) => _entity = entity;

        public override void Enter()
        {
            _entity.StartAttackRequest.Invoke();

            base.Enter();
        }
    }
}
