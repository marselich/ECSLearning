using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class TeleportWaitingState : State, IUpdatableState
    {
        private readonly ReactiveVariable<float> _interval;
        private readonly ICondition _isActiveMode;
        private float _elapsedTime;

        public TeleportWaitingState(ReactiveVariable<float> interval, ICondition isActiveMode)
        {
            _interval = interval;
            _isActiveMode = isActiveMode;
        }

        public bool IsReady => _isActiveMode.Evaluate() && _elapsedTime >= _interval.Value;

        public override void Enter()
        {
            base.Enter();
            _elapsedTime = 0;
        }

        public void Update(float deltaTime)
        {
            _elapsedTime = _isActiveMode.Evaluate()
                ? Mathf.Min(_elapsedTime + deltaTime, _interval.Value)
                : 0;
        }
    }
}
