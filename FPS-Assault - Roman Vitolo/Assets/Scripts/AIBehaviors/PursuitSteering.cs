using Interfaces;
using UnityEngine;

namespace AIBehaviours
{
    public abstract class PursuitParameters
    {
        public Transform _agent;
        public Transform _target;
        public float _velocity;
        public float _time; 
    }
    
    public class PursuitSteering : ISteeringBehaviour
    {
        private Transform _agent;
        private Transform _target;
        private float _velocity;
        private float _time;
        
        public PursuitSteering(PursuitParameters pursuitParameters)
        {
            _agent = pursuitParameters._agent;
            _target = pursuitParameters._target;
            _velocity = pursuitParameters._velocity;
            _time = pursuitParameters._time;
        }
        
        public Vector3 GetDirection()
        {   
            var direction = _target.position + _target.forward * (_velocity * _time);
            var dir = (direction - _agent.position).normalized;   
            Debug.Log($"Target position {dir}");
            return dir;
        }     
    }
}