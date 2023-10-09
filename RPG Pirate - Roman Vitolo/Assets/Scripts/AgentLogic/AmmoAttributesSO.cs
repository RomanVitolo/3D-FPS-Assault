using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/Agent Ammo Attributes", fileName = "New Agent Ammo")]
    public class AmmoAttributesSO : ScriptableObject
    {  
        [field: SerializeField] public float BulletSpeed { get; set; } 
        [field: SerializeField] public float BulletLifeTime { get; set; }     
    }
}