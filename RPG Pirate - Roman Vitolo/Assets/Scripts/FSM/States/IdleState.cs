using UnityEngine;

namespace FSM
{
    public class IdleState<T> : FSMState<T>
    {
        public IdleState()
        {
            
        }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Enter Idle State");
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}