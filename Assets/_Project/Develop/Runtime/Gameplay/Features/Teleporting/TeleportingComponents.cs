using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleporting
{
    public class TeleportingRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class IsTeleporting : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class TeleportProcessInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportProcessCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}