using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentLogic
{
    public class AgentLead : MonoBehaviour
    {
        [SerializeField] private GameEngine _gameEngine;
        [SerializeField] private AgentAttributes _agentAttributes;

        public List<GameObject> Agents = new List<GameObject>();

        private readonly List<AgentController> _agentControllers = new List<AgentController>();
        private BossController _myController;

        private void Awake()
        {
            _gameEngine = FindObjectOfType<GameEngine>();
            _myController = GetComponent<BossController>();
        }

        private void Start()
        {
            GetTeamAgents();
            //StartCoroutine(CommandAgent());
        }           

        private void GetTeamAgents()
        {
            foreach (var agent in _gameEngine.TeamAgents)
            {
                if (agent.Key == _agentAttributes.TeamName)
                {
                    var assignAgentsToCorrectTeam = agent.Value;   
                    foreach (var teamAgent in assignAgentsToCorrectTeam)
                    {   
                        Agents.Add(teamAgent);   
                    }          
                }
            }

            foreach (var teamAgents in Agents)
            {
                _agentControllers.Add(teamAgents.GetComponent<AgentController>()); 
            }
        }


        private IEnumerator CommandAgent()
        {
            while (true)
            {   
                yield return new WaitForSeconds(15f);  
                foreach (var agentController in _agentControllers)
                {
                    if (agentController == null)  
                        _agentControllers.Remove(agentController);
                    else if(agentController.CanDoANewQuestion(true))
                    {
                        Debug.Log("pip");
                        agentController.ExecuteTreeAgain();
                        agentController.CanDoANewQuestion(false); 
                    }       
                }
            }
        }
    }
}
