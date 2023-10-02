using Interfaces;
using UnityEngine;

namespace AIBehaviors
{
    public class HideSteering : ISteeringBehaviour
    {
        public Vector3 GetDirection()
        {
           Debug.Log("Hide Steering");
           return new Vector3(0, 0, 0);
        }
    }
}