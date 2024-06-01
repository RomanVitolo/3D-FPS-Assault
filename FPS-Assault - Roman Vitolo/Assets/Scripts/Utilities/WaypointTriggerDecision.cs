using AgentLogic;
using UnityEngine;

namespace Utilities
{
    public class WaypointTriggerDecision : MonoBehaviour
    {
        [SerializeField] private Collider _agent;     
        
        private void OnTriggerEnter(Collider other)
        {
            if (other != _agent) return;
            var executeTreeAgain = _agent.gameObject.GetComponent<AgentController>();    
            Debug.Log("Ready to execute new Tree");
            executeTreeAgain.ExecuteTreeAgain();
        }
    }
}