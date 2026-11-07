using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public interface IInputService
    {
        bool IsEnabled { get; }
        Vector3 Direction { get; }
        float MouseHorizontal { get; }
        bool AttackPressed { get; }
        bool MainMenuPressed { get; }
        bool DebugDamagePressed { get; }
    }
}
