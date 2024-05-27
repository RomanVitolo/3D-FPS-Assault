using AgentLogic;
using UnityEngine;

namespace Utilities
{
    public class WaypointTriggerDecision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var executeTreeAgain = other.gameObject.GetComponent<AgentAController>();
            executeTreeAgain.ExecuteTreeAgain();
        }
    }
}