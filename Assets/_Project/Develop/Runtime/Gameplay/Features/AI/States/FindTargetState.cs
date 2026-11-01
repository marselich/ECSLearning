using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Common;
using Assets._Project.Develop.Runtime.Gameplay.Features.Teleporting;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class FindTargetState : State, IUpdatableState
    {
        private readonly ITargetSelector _targetSelector;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly Transform _ownerTransform;
        private readonly ReactiveVariable<float> _targetDetectionRadius;
        private readonly ReactiveVariable<Entity> _currentTarget;

        public FindTargetState(ITargetSelector targetSelector, EntitiesLifeContext entitiesLifeContext, Entity entity)
        {
            _targetSelector = targetSelector;
            _entitiesLifeContext = entitiesLifeContext;
            _ownerTransform = entity.Transform;
            _targetDetectionRadius = entity.GetComponent<TargetDetectionRadius>().Value;
            _currentTarget = entity.CurrentTarget;
        }

        public void Update(float deltaTime)
        {
            _currentTarget.Value = _targetSelector.SelectTargetFrom(GetTargetsInRange());
        }

        private IEnumerable<Entity> GetTargetsInRange()
        {
            float radius = _targetDetectionRadius.Value;

            if (radius <= 0)
                yield break;

            float radiusSquared = radius * radius;

            foreach (Entity candidate in _entitiesLifeContext.Entities)
            {
                if (candidate.IsInit == false || candidate.HasComponent<TransformComponent>() == false)
                    continue;

                Vector3 offset = candidate.Transform.position - _ownerTransform.position;

                if (offset.sqrMagnitude <= radiusSquared)
                    yield return candidate;
            }
        }
    }
}
