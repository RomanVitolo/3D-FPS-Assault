using Interfaces;
using UnityEngine;

namespace FSM
{
    public class ChaseState<T> : FSMState<T>
    {
        private IAttack _pursuit;
        
        public ChaseState(IAttack pursuitBehavior)
        {
            _pursuit = pursuitBehavior;
        }

        public override void Enter()
        {
            Debug.Log("Chase State");
        }

        public override void Tick()
        {
            _pursuit.Pursuit();
        }
    }
}