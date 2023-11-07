using System.Collections.Generic;
using GameEngineClasses;
using UnityEngine;      

public class GameEngine : MonoBehaviour
{
     [SerializeField] private GameEngineEventsSO _gameEngineEventsSo;
     [SerializeField] private List<GameObject> _agents = new List<GameObject>();
   
     private void Awake()
     {
          if (_gameEngineEventsSo.CanSpawnAgents)
          {
               _gameEngineEventsSo.SpawnAgents(_agents);
          }                  
     }

     private void Start()
     {
          _gameEngineEventsSo.GameOver?.Invoke();
     }
}
