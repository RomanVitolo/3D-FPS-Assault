using System;
using UnityEngine;

namespace LineOfSight
{
    [CreateAssetMenu(menuName = "Entity/Line Of Sight", fileName = "Custom Line Of Sight")]
    public class LineOfSightConfigurationSO : ScriptableObject
    {
        [field: SerializeField] public bool InSight { get; set; }
        [field: SerializeField] public float FOVRange { get; private set; } 
        [field: SerializeField] public float FOVAngle{ get; private set; }
        [field: SerializeField] public LayerMask FOVLayerMask{ get; private set; }

        private void Awake() => InSight = false;   

        public bool IsInSight(Transform agent, Transform target, float range, float angle, LayerMask layerMask)
        {
            Vector3 diff = (target.position - agent.position);
            float distance = diff.magnitude; 
            if (distance > range) return false;
            
            float angleToTarget = Vector3.Angle(agent.forward, diff.normalized);
            if (angleToTarget > angle / 2) return false;

            if (Physics.Raycast(agent.position, diff.normalized, distance, layerMask))
            {
                return false;
            }
            return true;
        }
    }
}