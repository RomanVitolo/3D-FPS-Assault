using UnityEngine;

namespace Interfaces
{
    public interface IAttack
    {
        void Shoot();
        void Pursuit();
        void Reload(bool reload);
        void SwitchWeapon();            
    }
}