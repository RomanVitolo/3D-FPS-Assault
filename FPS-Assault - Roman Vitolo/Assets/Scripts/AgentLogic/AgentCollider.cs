using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AgentLogic
{
    public class AgentCollider : MonoBehaviour
    {
        [SerializeField] private AgentController _AgentController;
        [SerializeField] private AgentAI _agentAI;

        private void Awake()
        {
            _agentAI = GetComponent<AgentAI>();
            _AgentController = GetComponent<AgentController>();
        }        

        private void OnTriggerEnter(Collider other)
        {
            if (other != null)
            {
                var checkAgentLife = _agentAI.CheckLowLife();
                if (checkAgentLife)
                {
                    Debug.Log("New Tree Question");
                    _AgentController.ExecuteTreeAgain(); 
                }     
            }
        }
    }
}