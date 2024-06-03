using System.Collections.Generic;
using UnityEngine;

namespace GameEngineClasses
{
    [CreateAssetMenu(menuName = "Game Engine/InstantiateEntities/Agents", fileName = "Agent", order = 0)]
    public class InstantiateEntitiesSO : ScriptableObject
    {
        [field: SerializeField] public GameObject[] Agents { get; private set; }       
        [field: SerializeField] public string TeamName { get; private set; }    
        
        public void InstantiateAgents(Dictionary<string, List<GameObject>> teamAgents, Dictionary<string, List<Transform>> saveAgentsPosition)
        {     
            foreach (var agent in Agents)
            {
                GameObject instantiateAgentsPrefabs = Instantiate(agent, agent.transform.position, Quaternion.identity);
                if (teamAgents.ContainsKey(TeamName) && saveAgentsPosition.ContainsKey(TeamName))
                {
                    teamAgents[TeamName].Add(instantiateAgentsPrefabs);
                    saveAgentsPosition[TeamName].Add(instantiateAgentsPrefabs.transform);    
                }                   
                else
                {
                    teamAgents[TeamName] = new List<GameObject> {instantiateAgentsPrefabs};
                    saveAgentsPosition[TeamName] = new List<Transform> { instantiateAgentsPrefabs.transform }; 
                }  
                          
            }                     
        }
    }
}