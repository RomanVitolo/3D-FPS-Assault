using Interfaces;

namespace FSM
{  
    public class ShootState<T> : FSMState<T>
    {
        public IAttack _agentShoot;

        public ShootState(IAttack agentShoot)
        {
            _agentShoot = agentShoot;
        }

        public override void Tick()
        {
            _agentShoot.Shoot();
        }
    }
}