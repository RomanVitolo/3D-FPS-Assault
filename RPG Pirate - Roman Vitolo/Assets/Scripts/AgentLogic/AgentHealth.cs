using System;
using UnityEngine;

namespace AgentLogic
{
    public class AgentHealth : MonoBehaviour
    {      
        [SerializeField] private AgentHealthSO _agentHealth;

        private void Awake()
        {
            SetCurrentLife();
        }

        private void SetCurrentLife()
        {
            _agentHealth.CurrentAgentLife = _agentHealth.MaxAgentLife;
        }

        public int GetCurrentLife()
        {
            return _agentHealth.CurrentAgentLife;
        }
        
        public int TakeDamage(int damage)
        {
            var calculateDamage = _agentHealth.CurrentAgentLife - damage;
            return calculateDamage;
        }

        public bool IsAlive()
        {
            if (_agentHealth.CurrentAgentLife > 0)
            {
                return true;
            } 
            return false;
        }
    }
}