using System;
using UnityEngine;

namespace AgentLogic
{   
    public class AgentWeapon : MonoBehaviour
    {
        [SerializeField] private WeaponStatsSO _weaponStats;

        public float WeaponFireRate() => _weaponStats.FireRate;     

         public void Shoot()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, _weaponStats.WeaponShootRange, _weaponStats.KillLayer))
            {
                Debug.Log("Hit Entity" + hitInfo.transform.name);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * _weaponStats.WeaponShootRange);
        }   
        
    }
}