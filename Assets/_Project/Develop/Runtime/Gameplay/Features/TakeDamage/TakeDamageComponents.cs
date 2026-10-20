using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TakeDamage
{
    public class AreaOfEffectDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}