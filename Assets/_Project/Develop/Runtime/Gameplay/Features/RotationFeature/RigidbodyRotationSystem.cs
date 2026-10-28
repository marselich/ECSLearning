using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;
using Assets._Project.Develop.Runtime.Utilities.Conditions;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.RotationFeature
{
    public class RigidbodyRotationSystem : IInitializableSystem, IUpdatableSystem
    {
        private const float DeadZone = 0.0001f;

        private ReactiveVariable<Vector3> _rotationDirection;
        private ReactiveVariable<float> _rotationSpeed;
        private ICompositeCondition _canRotate;
        private Rigidbody _rigidbody;

        public void OnInit(Entity entity)
        {
            _rotationDirection = entity.RotationDirection;
            _rotationSpeed = entity.RotationSpeed;
            _canRotate = entity.CanRotate;
            _rigidbody = entity.Rigidbody;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!_canRotate.Evaluate() || _rotationDirection.Value.magnitude <= DeadZone)
                return;

            Quaternion lookRotation = Quaternion.LookRotation(_rotationDirection.Value);

            float step = _rotationSpeed.Value * deltaTime;

            _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, lookRotation, step);
        }
    }
}
