using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Utilities;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class FindTargetState : State, IUpdatableState
    {
        private readonly ITargetSelector _targetSelector;
        private readonly Buffer<Entity> _contacts;
        private readonly ReactiveVariable<Entity> _currentTarget;

        public FindTargetState(ITargetSelector targetSelector, Entity entity)
        {
            _targetSelector = targetSelector;
            _contacts = entity.ContactEntitiesBuffer;
            _currentTarget = entity.CurrentTarget;
        }

        public void Update(float deltaTime)
        {
            _currentTarget.Value = _targetSelector.SelectTargetFrom(GetSensorTargets());
        }

        private IEnumerable<Entity> GetSensorTargets()
        {
            for (int i = 0; i < _contacts.Count; i++)
                yield return _contacts.Items[i];
        }
    }
}
