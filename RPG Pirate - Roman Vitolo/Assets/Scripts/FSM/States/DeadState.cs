using Interfaces;
using UnityEngine;

namespace FSM
{
    public class DeadState<T> : FSMState<T>
    {
        private IEntity _entity;
        public DeadState(IEntity entity)
        {
            _entity = entity;
        }

        public override void Enter()
        {
            Debug.Log("Player is Dead");
            _entity.Dead();
        }

        public override void Tick()
        {
            Debug.Log("Player is Dead");
        }
    }
}