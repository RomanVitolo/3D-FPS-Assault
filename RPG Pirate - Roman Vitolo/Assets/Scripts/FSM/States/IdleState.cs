using Interfaces;
using UnityEngine;

namespace FSM
{
    public class IdleState<T> : FSMState<T>
    {
        private IMove _agent;
        public IdleState(IMove agent)
        {
            _agent = agent;
        }   

        public override void Tick()
        {
           _agent.Idle();
        }   
       
    }
}