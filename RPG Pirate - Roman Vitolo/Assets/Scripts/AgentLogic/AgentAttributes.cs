using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/EnemyStats", fileName = "AgentAttribute")]
    public class AgentAttributes : ScriptableObject
    {
       public string Name = Guid.NewGuid().ToString();
       
       [field: SerializeField] public bool IsLeader { get; private set; }
       [field: SerializeField] public float MaxAgentLife { get; set; } 
       [field: SerializeField] public float CurrentAgentLife { get; set; } 
       [field: SerializeField] public float Speed { get; set; }
       [field: SerializeField] public bool HasBullet { get; set; } 
       [field: SerializeField] public bool InitialBullets { get; set; } 
       [field: SerializeField] public int AmountOfBullets { get; set; }  
       [field: SerializeField] public float FireRate { get; set; } 
       [field: SerializeField] public float ReloadSpeed{ get; private set; }
       [field: SerializeField] public List<string> WeaponType { get; set; } 


       public void InitializeWeaponType(string weaponType)
       {
           WeaponType = new List<string> {weaponType};
       }
    }
}