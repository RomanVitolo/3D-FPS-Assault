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
    }
}