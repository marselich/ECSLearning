using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class MaxEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CurrentEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class IsEnergyRestore : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class EnergyExpend : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRestorationPercent : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRestorationInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRestorationCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}