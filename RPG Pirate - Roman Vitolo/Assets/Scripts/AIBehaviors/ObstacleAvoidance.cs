using Interfaces;
using UnityEngine;

namespace FSM
{
    public class ObstacleAvoidance : ISteeringBehaviour
    {
        public Transform _agent;
        public Transform _target;
        public float _radius;
        public LayerMask _layerMask;  
        public float _avoidWeight;

        public ObstacleAvoidance(Transform agent, Transform target, float radius, LayerMask layerMask, float avoidWeight)
        {
            _agent = agent;
            _radius = radius;
            _layerMask = layerMask;
            _target = target;
            _avoidWeight = avoidWeight;
        }
    
        public Vector3 GetDirection()
        {
            Collider[] obstacles = Physics.OverlapSphere(_agent.position, _radius, _layerMask);
            Transform obsSave = null;
            var count = obstacles.Length;
            for (int i = 0; i < count; i++)
            {
                var currObs = obstacles[i].transform;
                if (obsSave == null)
                {
                    obsSave = obstacles[i].transform;
                }
                else if (Vector3.Distance(_agent.position, obsSave.position) > Vector3.Distance(_agent.position, currObs.position))
                {
                    obsSave = currObs;
                }
            }
            Vector3 dirToTarget = (_target.position - _agent.position).normalized;
            if (obsSave != null)
            {
                Vector3 dirObsToObstacle = (_agent.position - obsSave.position).normalized * _avoidWeight;
                dirToTarget += dirObsToObstacle;
            }
            return dirToTarget.normalized;
        }
    }
}