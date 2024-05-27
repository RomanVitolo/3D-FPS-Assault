using Interfaces;

namespace FSM
{
    public class IdleState<T> : FSMState<T>
    {
        private IMove _agent;
        public IdleState(IMove agent)
        {
            _agent = agent;
        }

        public override void Enter()
        {
            
        }

        public override void Tick()
        {
            _agent.Idle();
        }
    }
}