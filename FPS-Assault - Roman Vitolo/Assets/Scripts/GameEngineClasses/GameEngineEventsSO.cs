using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEngineClasses
{
    [CreateAssetMenu(menuName = "GameEngine Events", fileName = "New Custom Event")]
    public class GameEngineEventsSO : ScriptableObject
    { 
       [field: SerializeField] public UnityEvent StartGame { get; set; }
       [field: SerializeField] public UnityEvent GameOver { get; set; } 
       [field: SerializeField] public bool CanSpawnAgents { get; set; } 
             
        public void SpawnAgents(List<GameObject> agents)
        {
            foreach (var agent in agents)
            {
                Instantiate(agent, agent.transform.position, Quaternion.identity);      
            }
        }    
    }
}