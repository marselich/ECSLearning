using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class LowestHealthTargetSelector : ITargetSelector
    {
        private readonly Entity _owner;

        public LowestHealthTargetSelector(Entity owner)
        {
            _owner = owner;
        }

        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
        {
            Entity target = null;
            float lowestHealth = float.PositiveInfinity;

            foreach (Entity entitiy in targets)
            {
                if (entitiy == null || entitiy == _owner || entitiy.IsInit == false
                    || entitiy.HasComponent<CurrentHealth>() == false
                    || entitiy.CurrentHealth.Value <= 0
                    || (entitiy.HasComponent<IsDead>() && entitiy.IsDead.Value))
                    continue;

                if (entitiy.CurrentHealth.Value < lowestHealth)
                {
                    lowestHealth = entitiy.CurrentHealth.Value;
                    target = entitiy;
                }
            }

            return target;
        }
    }
}
