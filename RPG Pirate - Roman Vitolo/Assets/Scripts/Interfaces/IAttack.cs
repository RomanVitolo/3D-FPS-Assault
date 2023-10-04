using UnityEngine;

namespace Interfaces
{
    public interface IAttack
    {
        void Shoot();
        void Pursuit();
        void Reload();
        void SwitchWeapon();
    }
}