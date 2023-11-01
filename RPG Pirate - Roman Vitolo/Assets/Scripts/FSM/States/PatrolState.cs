using Interfaces;
using UnityEngine;

namespace FSM
{
    public class PatrolState<T> : FSMState<T>
    {
        private IMove _entity;
        
        public PatrolState(IMove agent)
        {
            _entity = agent;
        }                            

        public override void Tick()
        {
            Debug.Log("PatrolState");
        }
    }
}