using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class DesktopInput : IInputService
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";

        public bool IsEnabled { get; set; } = true;
        public float MouseHorizontal => IsEnabled ? Input.GetAxisRaw("Mouse X") : 0;
        public bool AttackPressed => IsEnabled && Input.GetMouseButtonDown(0);
        public bool MainMenuPressed => IsEnabled && Input.GetKeyDown(KeyCode.F);
        public bool DebugDamagePressed => IsEnabled && Input.GetKeyDown(KeyCode.K);

        public Vector3 Direction
        {
            get
            {
                if (IsEnabled == false)
                    return Vector3.zero;

                return new Vector3(Input.GetAxisRaw(HorizontalAxisName), 0, Input.GetAxisRaw(VerticalAxisName));
            }
        }
    }
}
