using System.Collections.Generic;
using UnityEngine;

namespace GameEngineClasses
{
    [CreateAssetMenu(menuName = "InstantiateEntities/Agents", fileName = "Agent", order = 0)]
    public class InstantiateEntitiesSO : ScriptableObject
    {
        [field: SerializeField] public GameObject[] Agents { get; private set; }           
        [field: SerializeField] public GameObject Boss { get; private set; }
        
        public void InstantiateAgents(List<Transform> saveAgents)
        {
            foreach (var agent in Agents)
            {
                GameObject instantiateAgentsPrefabs = Instantiate(agent, agent.transform.position, Quaternion.identity) as GameObject;
                saveAgents.Add(instantiateAgentsPrefabs.transform);
            }                 
            
            GameObject bossPrefab = Instantiate(Boss, Boss.transform.position, Quaternion.identity);
            saveAgents.Add(bossPrefab.transform);      
        }
    }
}