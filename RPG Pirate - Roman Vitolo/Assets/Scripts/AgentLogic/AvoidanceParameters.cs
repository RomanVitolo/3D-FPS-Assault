using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/ObstacleAvoidanceSO", fileName = "Custom Obstacle Avoidance")]
    public class AvoidanceParameters : ScriptableObject
    {  
        [field: SerializeField] public float Radius { get; private set; } 
        [field: SerializeField] public LayerMask ObstacleMask { get; private set; }   
        [field: SerializeField] public float AvoidWeight { get; private set; }    
    }
}