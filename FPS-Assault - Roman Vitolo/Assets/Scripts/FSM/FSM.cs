using UnityEngine;

namespace FSM
{
    public class FSM<T>
    {
        private FSMState<T> _current; 
        
        public void InitializeFSM(FSMState<T> init)
        {
            _current = init;
            _current.Enter();
        }

        public void OnTick()
        {
            _current.Tick();
        }

        public void Transition(T input)
        {
           FSMState<T> newState = _current.GetTransition(input);
           if (newState == null) return;
           
           _current.Exit();
           _current = newState;
           _current.Enter(); 
        }
    }
}