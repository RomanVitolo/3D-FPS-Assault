using Interfaces;
using UnityEngine;

namespace AIBehaviours
{
    public class PursuitSteering : ISteeringBehaviour
    {
        public Vector3 GetDirection()
        {
            Debug.Log("GetDirection");
            return new Vector3(0, 0, 0);
        }
    }
}