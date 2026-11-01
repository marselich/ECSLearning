using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleporting
{
    public class TeleportingRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportInterval : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TargetTeleportInterval : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TargetDetectionRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportDestination : IEntityComponent
    {
        public ReactiveVariable<Vector3> Value;
    }

    public class IsTeleporting : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class CanTeleport : IEntityComponent
    {
        public ICompositeCondition Value;
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
