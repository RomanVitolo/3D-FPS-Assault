using Interfaces;

namespace FSM
{
    public class SwitchWeaponState<T> : FSMState<T>
    {
        private IAttack _switchWeapon;
        
        public SwitchWeaponState(IAttack newWeapon)
        {
            _switchWeapon = newWeapon;
        }

        public override void Enter()
        {
            _switchWeapon.SwitchWeapon();
        }
    }
}