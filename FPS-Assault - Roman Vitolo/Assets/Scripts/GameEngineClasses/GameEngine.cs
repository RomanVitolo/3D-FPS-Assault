using System.Collections.Generic;
using GameEngineClasses;
using UnityEngine;     


public class GameEngine : MonoBehaviour
{                                        
     public Dictionary<string, List<Transform>> TeamAgentsPosition { get; set; } = new Dictionary<string, List<Transform>>();
     public Dictionary<string, List<GameObject>> TeamAgents { get; set; } = new Dictionary<string, List<GameObject>>();
     
     [SerializeField] private GameEngineEventsSO _gameEngineEvents; 
     [SerializeField] private InstantiateEntitiesSO[] _instantiateAgents;
     
     private void Awake()
     {
          SpawnAgents();
     }

     private void Start()
     {
          _gameEngineEvents.GameOver?.Invoke();  
     }          
     
     private void SpawnAgents()
     {       
          foreach (var agents in _instantiateAgents)
          {
               agents.InstantiateAgents(TeamAgents, TeamAgentsPosition);
          }
     }    
}
