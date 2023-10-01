using System.Collections.Generic;

namespace FSM
{  
    public class FSMState<T>
    {
        private Dictionary<T, FSMState<T>> _states = new Dictionary<T, FSMState<T>>();
        
        public virtual void Enter() {}
        public virtual void Tick() {}
        public virtual void Exit() {}  

        public void AddTransition(T input, FSMState<T> state)
        {
            _states.TryAdd(input, state);
        }

        public void RemoveTransition(T input, FSMState<T> state)
        {
            if (_states.ContainsKey(input))
                _states.Remove(input);
        }

        public FSMState<T> GetTransition(T input)
        {
            if (_states.TryGetValue(input, out var transition))
            {
                return transition;
            } 
            return null;
        }
    }
}