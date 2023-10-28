using System.Collections.Generic;       
using Interfaces;
using UnityEngine;

namespace AIBehaviors
{
    public class HideSteering : ISteeringBehaviour
    {
        private Transform _agent;
        private Transform _nearestWaypoint;
        private Transform _target;
        private float _moveSpeed;
        private List<Transform> _waypoints;
        private bool _canMove;
        
        private Vector3 _agentPosition = Vector3.zero;

        public HideSteering(Transform agent, Transform nearestWaypoint, Transform target, float moveSpeed, List<Transform> waypoints, bool canMove)
        {
            _agent = agent;
            _nearestWaypoint = nearestWaypoint;
            _target = target;
            _moveSpeed = moveSpeed;
            _waypoints = waypoints;
            _canMove = canMove;
        }
        
        public Vector3 GetDirection()
        {
            _agentPosition = _agent.position;
            _agentPosition.y = 0;    
            
            if (_canMove == false) 
                return Vector3.zero;
           
            FindNearestWaypoint();     
            Vector3 steeringDirection = SteerTowardsWaypoint(_nearestWaypoint.position);
            
            if (Vector3.Distance(_agent.position, _nearestWaypoint.position) < 2f)    
                _canMove = false;
            
            return steeringDirection;
        }         
        
        private void FindNearestWaypoint()
        {
            float minDistance = float.MaxValue;

            foreach (Transform waypoint in _waypoints)
            {
                float distance = Vector3.Distance(_agent.position, waypoint.position);    
                
                float playerDistance = Vector3.Distance(_agent.position, _target.position);      
                
                if (playerDistance > 2.0f && distance < minDistance) 
                {
                    minDistance = distance;
                    _nearestWaypoint = waypoint;   
                }           
            }
        }

        private Vector3 SteerTowardsWaypoint(Vector3 waypointPosition)
        {
            Vector3 desiredDirection = waypointPosition - _agent.position;
            desiredDirection.Normalize();
            Vector3 desiredVelocity = desiredDirection * _moveSpeed;
            Vector3 steeringForce = desiredVelocity - _agent.forward;   
            return steeringForce;
        }
    }
}