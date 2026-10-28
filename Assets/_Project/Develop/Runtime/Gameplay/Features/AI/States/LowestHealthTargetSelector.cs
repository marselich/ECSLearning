using Assets._Project.Develop.Runtime.Gameplay.Common;
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
            foreach (Entity candidate in targets)
            {
                if (candidate == null || candidate == _owner || candidate.IsInit == false
                    || candidate.HasComponent<CurrentHealth>() == false
                    || candidate.HasComponent<TransformComponent>() == false
                    || candidate.CurrentHealth.Value <= 0
                    || (candidate.HasComponent<IsDead>() && candidate.IsDead.Value))
                    continue;

                if (candidate.CurrentHealth.Value < lowestHealth)
                {
                    lowestHealth = candidate.CurrentHealth.Value;
                    target = candidate;
                }
            }
            return target;
        }
    }
}
