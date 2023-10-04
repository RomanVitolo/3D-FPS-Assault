using Interfaces;
using UnityEngine;

namespace AIBehaviours
{
    public class PursuitSteering : ISteeringBehaviour
    {
        private Transform _agent;
        private Transform _target;
        private float _velocity;
        private float _time;
        
        public PursuitSteering(Transform agent, Transform target, float velocity, float time)
        {
            _agent = agent;
            _target = target;
            _velocity = velocity;
            _time = time;
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