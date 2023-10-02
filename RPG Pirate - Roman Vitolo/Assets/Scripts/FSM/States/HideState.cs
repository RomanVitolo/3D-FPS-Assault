using Interfaces;
using UnityEngine;

namespace FSM
{
    public class HideState<T> : FSMState<T>
    {
        private IMove _entity;
        public HideState(IMove agent)
        {
            _entity = agent; 
        }

        public override void Enter()
        {
            Debug.Log("Enter in Hide State");
        }

        public override void Tick()
        {
            _entity.Move();
        }
    }
}