using System;
using Interfaces;
using UnityEngine;    

namespace AgentLogic
{
    [CreateAssetMenu(menuName = "Entity/EnemyStats", fileName = "AgentAttribute")]
    public class AgentAttributes : ScriptableObject
    {
       public string NameTag;
       public string TeamName;
       
       [field: SerializeField] public bool IsLeader { get; private set; }
       [field: SerializeField] public float AgentSpeed { get; set; }
       [field: SerializeField] public float AgentTurnSpeed { get; set; }
       [field: SerializeField] public string WaypointNameTag { get; private set; }
    }
}