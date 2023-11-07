using Interfaces;
using UnityEngine;

namespace FSM
{
    public class HideState<T> : FSMState<T>
    {
        private IMove _entity;
        private bool _canMove;
        public HideState(IMove agent, bool canMove)
        {
            _entity = agent;
            _canMove = canMove;
        }

        public override void Enter()
        {
            Debug.Log("Enter in Hide State");
        }

        public override void Tick()
        {
            if (_canMove)
            {
                _entity.Hide();
            }
        }
    }
}