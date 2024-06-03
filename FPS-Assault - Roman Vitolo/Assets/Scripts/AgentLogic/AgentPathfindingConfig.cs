using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AgentLogic
{
    public class AgentPathfindingConfig : MonoBehaviour
    {
        public string WaypointTag; 
        public float radius;
        public Vector3 offset;
        public Node init;
        public Node finit;
        public AgentAI _agent;   
        private List<Node> _list;
        private List<Vector3> _listVector;    
        private AStar<Node> _aStar = new AStar<Node>();
        
        [SerializeField] private List<Node> RandomInitNode = new List<Node>();
        [SerializeField] private List<Node> RandomFinitNode = new List<Node>();  
        

        private void Awake()
        {
            _agent = GetComponent<AgentAI>();
        }

        private void OnEnable()
        {
            FindInitNode();
        }

        private void FindInitNode()
        {
            var waypoints = GameObject.FindGameObjectsWithTag(WaypointTag);
            foreach (var waypoint in waypoints)
            {
                var getComponent = waypoint.GetComponent<Node>();
                if (getComponent.CanBeInitialNode && getComponent.CanBeEndNode == false)
                {
                    RandomInitNode.Add(getComponent);
                    var randomInitWaypoint = Random.Range(0, RandomInitNode.Count);
                    init = RandomInitNode[randomInitWaypoint];
                }
                else if (getComponent.CanBeInitialNode == false && getComponent.CanBeEndNode)
                {
                    RandomFinitNode.Add(getComponent);
                    var randomFinitNode = Random.Range(0, RandomFinitNode.Count);
                    finit = RandomFinitNode[randomFinitNode];
                } 
            }                   
        }

        public void PathFindingAStar()
        {
            _list = _aStar.Run(init, Satisfies, GetNeighbours, GetCost, Heuristic);   
            _agent.SetWayPoints(_list);    
        }                  
    
        float Heuristic(Node curr)
        {                         
            float cost = 0;     
            cost += Vector3.Distance(curr.transform.position, finit.transform.position);
            return cost;
        }
        
        float GetCost(Node p, Node c) => Vector3.Distance(p.transform.position, c.transform.position);    
        
        List<Node> GetNeighbours(Node curr) => curr.Neightbourds;      
        bool Satisfies(Node curr) => curr == finit;
     
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (init != null)
                Gizmos.DrawSphere(init.transform.position + offset, radius);
            if (finit != null)
                Gizmos.DrawSphere(finit.transform.position + offset, radius);
            if (_list != null)
            {
                Gizmos.color = Color.blue;
                foreach (var item in _list)
                {
                    if (item != init && item != finit)
                        Gizmos.DrawSphere(item.transform.position + offset, radius);
                }
            }
            if (_listVector != null)
            {
                Gizmos.color = Color.green;
                foreach (var item in _listVector)
                {
                    if (item != init.transform.position && item != finit.transform.position)
                        Gizmos.DrawSphere(item + offset, radius);
                }
            }
        } 
    }
}