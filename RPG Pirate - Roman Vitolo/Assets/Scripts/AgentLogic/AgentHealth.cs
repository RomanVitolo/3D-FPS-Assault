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
        
        public void TakeDamage(int damage)
        {
            if (_agentHealth.CurrentAgentLife < 0)
            {
                var calculateDamage = _agentHealth.CurrentAgentLife - damage;
                _agentHealth.CurrentAgentLife = calculateDamage;
            }
            else
            {
                Destroy(this.gameObject, 2.5f);
            }
            
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