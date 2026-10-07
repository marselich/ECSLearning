using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.RotationFeature
{
    public class TransformRotationSystem : IInitializableSystem, IUpdatableSystem
    {
        private const float DeadZone = 0.5f;

        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _rotationSpeed;
        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _rotationSpeed = entity.RotationSpeed;
            _transform = entity.Transform;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_moveDirection.Value.magnitude <= DeadZone)
                return;

            Quaternion lookRotation = Quaternion.LookRotation(_moveDirection.Value);

            float step = _rotationSpeed.Value * deltaTime;

            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, lookRotation, step);
        }
    }
}
