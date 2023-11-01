using System; 
using UnityEngine;    

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/EnemyStats", fileName = "AgentAttribute")]
    public class AgentAttributes : ScriptableObject
    {
       public string Name = Guid.NewGuid().ToString();
       
       [field: SerializeField] public bool IsLeader { get; private set; }
       [field: SerializeField] public float AgentSpeed { get; set; }
       [field: SerializeField] public float AgentTurnSpeed { get; set; }
       [field: SerializeField] public bool HasBullet { get; set; } 
       [field: SerializeField] public bool InitialBullets { get; set; } 
       [field: SerializeField] public int AmountOfBullets { get; set; }  
       [field: SerializeField] public float FireRate { get; set; } 
       [field: SerializeField] public float ReloadSpeed { get; private set; } 
       [field: SerializeField] public string WaypointNameTag { get; private set; } 
    }
}