﻿using System;
using UnityEngine;

namespace AgentLogic
{
    public class AgentHealth : MonoBehaviour
    {
        [SerializeField] private AgentAttributes _agentAttributes;

        private void Awake()
        {
            SetCurrentLife();
        }

        private void SetCurrentLife()
        {
             _agentAttributes.CurrentAgentLife = _agentAttributes.MaxAgentLife;
        }

        public float GetCurrentLife()
        {
            return _agentAttributes.CurrentAgentLife;
        }
        
        public float TakeDamage(float damage)
        {
            var calculateDamage = _agentAttributes.CurrentAgentLife - damage;
            return calculateDamage;
        }

        public bool IsAlive()
        {
            if (_agentAttributes.CurrentAgentLife > 0)
            {
                return true;
            } 
            return false;
        }
    }
}