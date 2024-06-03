using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/ObstacleAvoidance", fileName = "Custom Obstacle Avoidance")]
    public class AvoidanceParametersSO : ScriptableObject
    {  
        [field: SerializeField] public float Radius { get; private set; } 
        [field: SerializeField] public LayerMask ObstacleMask { get; private set; }   
        [field: SerializeField] public float AvoidWeight { get; private set; }    
    }
}