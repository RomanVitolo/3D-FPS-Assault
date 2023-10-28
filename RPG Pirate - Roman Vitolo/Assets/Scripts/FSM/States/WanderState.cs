using Interfaces;

namespace FSM
{
    public class WanderState<T> : FSMState<T>
    {
        private IMove _agent;
        public WanderState(IMove agent)
        {
            _agent = agent;
        }

        public override void Enter()
        {
            
        }

        public override void Tick()
        {
            _agent.Wander();
        }
    }
}