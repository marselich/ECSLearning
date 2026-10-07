using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class CharacterControllerMovementSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _moveSpeed;
        private CharacterController _characterController;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _moveSpeed = entity.MoveSpeed;
            _characterController = entity.CharacterController;
        }

        public void OnUpdate(float deltaTime)
        {
            Vector3 velocity = _moveDirection.Value.normalized * _moveSpeed.Value;

            Debug.Log("Применяемая скорость: " + velocity.ToString());

            _characterController.Move(velocity * deltaTime);
        }
    }
}