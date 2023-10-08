using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace AIBehaviors
{
    public class HideSteering : ISteeringBehaviour
    {
        public Transform _agent;
        public Transform _nearestWaypoint;
        public Transform _target;
        public float _moveSpeed;
        public List<Transform> _waypoints;

        public HideSteering(Transform agent, Transform nearestWaypoint, Transform target, float moveSpeed, List<Transform> waypoints)
        {
            _agent = agent;
            _nearestWaypoint = nearestWaypoint;
            _target = target;
            _moveSpeed = moveSpeed;
            _waypoints = waypoints;
        }
        
        public Vector3 GetDirection()
        { 
           FindNearestWaypoint();
          return SteerTowardsWaypoint(_nearestWaypoint.position);
        }
        
        private void FindNearestWaypoint()
        {
            float minDistance = float.MaxValue;

            foreach (Transform waypoint in _waypoints)
            {
                float distance = Vector3.Distance(_agent.position, waypoint.position);
                if (distance < minDistance)
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