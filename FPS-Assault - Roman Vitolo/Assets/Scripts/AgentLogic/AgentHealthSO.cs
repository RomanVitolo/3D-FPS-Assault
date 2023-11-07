using UnityEngine;

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/Agent Health Attributes", fileName = "New Agent Health")]
    public class AgentHealthSO : ScriptableObject
    {
        [field: SerializeField] public int MaxAgentLife { get; set; } 
        [field: SerializeField] public int CurrentAgentLife { get; set; } 
    }
}