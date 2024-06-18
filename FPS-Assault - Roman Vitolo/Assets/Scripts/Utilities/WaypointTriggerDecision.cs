using Interfaces;
using UnityEngine;

namespace Utilities
{
    public class WaypointTriggerDecision : MonoBehaviour
    {    
        private void OnTriggerEnter(Collider other)
        {
            if (other != null) ;
            Debug.Log("Agent is Ready");
            var agentReady = other.GetComponent<IReady>();
            agentReady.CanDoANewQuestion(true);
        }     
    }
}