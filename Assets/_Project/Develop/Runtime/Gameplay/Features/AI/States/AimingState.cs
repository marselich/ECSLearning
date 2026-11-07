using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    internal class AimingState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly IInputService _input;

        public AimingState(Entity entity, IInputService input)
        {
            _entity = entity;
            _input = input;
        }

        public void Update(float deltaTime)
        {
            float angle = _input.MouseHorizontal * _entity.MouseSensitivity.Value;
            _entity.RotationDirection.Value = Quaternion.AngleAxis(angle, Vector3.up) * _entity.Transform.forward;
        }
    }
}
