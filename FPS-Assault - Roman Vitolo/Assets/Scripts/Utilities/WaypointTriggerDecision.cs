using System;
using System.Collections;
using AgentLogic;
using Interfaces;
using UnityEngine;

namespace Utilities
{
    public class WaypointTriggerDecision : MonoBehaviour
    {    
        private void OnTriggerEnter(Collider other)
        {
            if (other != null) ;
            Debug.Log("Ready to execute new Tree");
            var agentReady = other.GetComponent<IReady>();
            agentReady.CanDoANewQuestion(true);
        }     
    }
}