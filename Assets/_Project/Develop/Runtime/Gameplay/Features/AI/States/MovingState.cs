using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    internal class MovingState : State, IUpdatableState
    {
        private readonly Entity _entity;
        private readonly IInputService _input;

        public MovingState(Entity entity, IInputService input)
        {
            _entity = entity;
            _input = input;
        }

        public void Update(float deltaTime)
        {
            Vector3 direction = _input.Direction;
            _entity.MoveDirection.Value = direction;
            _entity.RotationDirection.Value = direction;
        }

        public override void Exit()
        {
            base.Exit();

            _entity.MoveDirection.Value = Vector3.zero;
        }
    }
}
