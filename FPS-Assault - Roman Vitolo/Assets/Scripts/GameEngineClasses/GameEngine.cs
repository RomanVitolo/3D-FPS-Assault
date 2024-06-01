using System.Collections.Generic;
using GameEngineClasses;
using UnityEngine;     


public class GameEngine : MonoBehaviour
{                                         
     [field: SerializeField] public List<Transform> TeamAAgents { get; private set; } = new List<Transform>();
     [field: SerializeField] public List<Transform> TeamBAgents { get; private set; }= new List<Transform>();
     
     [SerializeField] private GameEngineEventsSO _gameEngineEvents;
     [SerializeField] private InstantiateEntitiesSO _instantiateTeamAAgents;
     [SerializeField] private InstantiateEntitiesSO _instantiateTeamBAgents;
     
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
          _instantiateTeamBAgents.InstantiateAgents(TeamBAgents);
          _instantiateTeamAAgents.InstantiateAgents(TeamAAgents);   
     }    
}
