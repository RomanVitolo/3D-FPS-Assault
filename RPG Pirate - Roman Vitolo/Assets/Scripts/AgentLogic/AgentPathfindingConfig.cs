using System;
using System.Collections.Generic;
using UnityEngine;

namespace AgentLogic
{
    public class AgentPathfindingConfig : MonoBehaviour
    {             
        public float radius;
        public Vector3 offset;
        public Node init;
        public Node finit;
        public AgentAI _agent;   
        private List<Node> _list;
        private List<Vector3> _listVector;    
        private AStar<Node> _aStar = new AStar<Node>();

        private void Awake()
        {
            _agent = GetComponent<AgentAI>();
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