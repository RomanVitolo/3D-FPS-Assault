using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/Agent Weapon Stats", fileName = "New Weapon")]
    public class WeaponStatsSO : ScriptableObject
    {
        [field: SerializeField] public float WeaponShootRange { get; private set; }
        [field: SerializeField] public LayerMask KillLayer { get; private set; }
        [field: SerializeField] public int WeaponDamage { get; private set; }          
        [field: SerializeField] public int InitialWeaponAmmo { get;  set; }    
        [field: SerializeField] public int CurrentAmmo { get; set; } 
        [field: SerializeField] public float FireRate { get; private set; }                 
        
    }
}