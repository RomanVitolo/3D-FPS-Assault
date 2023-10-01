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

        public Vector3 GetDirection()
        {
            throw new System.NotImplementedException();
        }
    }
}