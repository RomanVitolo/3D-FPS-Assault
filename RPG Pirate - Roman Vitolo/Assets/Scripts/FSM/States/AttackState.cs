using Interfaces;
using UnityEngine;

namespace FSM
{  
    public class AttackState<T> : FSMState<T>
    {
        public IAttack _agentShoot;

        public AttackState(IAttack agentShoot)
        {
            _agentShoot = agentShoot;
        }

        public override void Tick()
        {
            _agentShoot.Shoot();
        }
    }
}