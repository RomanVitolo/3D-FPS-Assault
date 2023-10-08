using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/Agent Health Attributes", fileName = "New Agent Helath")]
    public class AgentHealthSO : ScriptableObject
    {
        [field: SerializeField] public float MaxAgentLife { get; set; } 
        [field: SerializeField] public float CurrentAgentLife { get; set; } 
    }
}