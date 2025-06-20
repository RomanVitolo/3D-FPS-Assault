﻿using System.Collections.Generic;
using AgentLogic;
using Interfaces;
using UnityEngine;

namespace FSM
{
    public class PatrolState<T> : FSMState<T>
    {
        private IMove _agent;
        private AgentPathfindingConfig _agentPathfinding;
        
        public PatrolState(IMove agent, AgentPathfindingConfig agentPathfinding)
        {
            _agentPathfinding = agentPathfinding;
            _agent = agent;  
        }

        public override void Enter()
        {
            _agentPathfinding.PathFindingAStar();       
        }

        public override void Tick()
        {
            Debug.Log("PatrolState");
            _agent.Run();
        }    
    }
}