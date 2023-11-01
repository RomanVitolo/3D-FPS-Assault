using System;
using System.Collections;
using System.Collections.Generic;
using AIBehaviors;  
using DecisionTree;
using DefaultNamespace;
using FSM;
using LineOfSight;
using UnityEngine;

namespace AgentLogic
{
    public class AgentAController : MonoBehaviour
    {
        [SerializeField] private AgentAI _agentAI;
        [SerializeField] private Transform _target;
        [SerializeField] private LineOfSightConfigurationSO _agentSight;
        [SerializeField] private AvoidanceParametersSO _avoidanceParametersSo;
        private List<Transform> _waypoints = new List<Transform>();

        private INode _initTree;
        private FSM<string> _fsm;  
        private Transform _nearestWeapon;
        private Roulette _roulette;
        private Dictionary<string, int> _randomDecision = new Dictionary<string, int>();     

        private void Awake()
        {
            _agentAI = GetComponent<AgentAI>();   
        }

        private void Start()
        {
            FindWaypoints();
            
            _roulette = new Roulette();  
            _randomDecision.Add("Shoot", 20);
            _randomDecision.Add("Chase", 70);    
            _randomDecision.Add("Patrol", 10);  
            
            _fsm = new FSM<string>();

            DeadState<string> deadState = new DeadState<string>(_agentAI);
            WanderState<string> wanderState = new WanderState<string>(_agentAI);
            PatrolState<string> patrolState = new PatrolState<string>(_agentAI);
            ChaseState<string> chaseState = new ChaseState<string>(_agentAI);
            HideState<string> hideState = new HideState<string>(_agentAI, _agentAI.CanMove);
            ReloadState<string> reloadState = new ReloadState<string>(_agentAI);
            ShootState<string> shootState = new ShootState<string>(_agentAI);    

            _fsm.InitializeFSM(wanderState);
            
            wanderState.AddTransition("Chase", chaseState);  
            wanderState.AddTransition("Hide", hideState);  
            wanderState.AddTransition("Dead", deadState);  
            wanderState.AddTransition("Reload", reloadState);  
            wanderState.AddTransition("Shoot", shootState);  
            wanderState.AddTransition("Patrol", patrolState);  
            
            chaseState.AddTransition("Wander", wanderState);
            chaseState.AddTransition("Hide", hideState);
            chaseState.AddTransition("Dead", deadState); 
            chaseState.AddTransition("Shoot", shootState); 
            
            hideState.AddTransition("Wander", wanderState);
            hideState.AddTransition("Chase", chaseState);
            hideState.AddTransition("Dead", deadState); 
            hideState.AddTransition("Patrol", patrolState); 
            
            reloadState.AddTransition("Wander", wanderState);
            reloadState.AddTransition("Shoot", shootState);
            
            shootState.AddTransition("Wander", wanderState);
            shootState.AddTransition("Chase", chaseState);        
            
            patrolState.AddTransition("Wander", wanderState);
            
            ActionNode dead = new ActionNode(AgentIsDead); 
            ActionNode chase = new ActionNode(ChaseEnemy);
            ActionNode hide = new ActionNode(HideFromEnemy);
            ActionNode attack = new ActionNode(ShootState);
            ActionNode patrol = new ActionNode(Patrol);           
            ActionNode reload = new ActionNode(ReloadState);           
            ActionNode wander = new ActionNode(Wander);          
                                                         
            QuestionNode hasAmmo = new QuestionNode(EnemyIsInRange, attack, reload);    
            QuestionNode isInRange = new QuestionNode(EnemyIsInRange, hasAmmo, wander);
            QuestionNode canChase = new QuestionNode(EnemyIsInRange, hasAmmo, wander);
            QuestionNode canWander = new QuestionNode(EnemyIsInRange, chase, patrol);
            QuestionNode hidePath = new QuestionNode(_agentAI.HideOrWander, hide, canWander);
            QuestionNode hasLowLife = new QuestionNode(_agentAI.CheckLowLife, hidePath, canWander);
            QuestionNode hasLife = new QuestionNode(_agentAI.CheckLife, hasLowLife, dead);  

            _initTree = hasLife;
            _initTree.Execute();
            
            _agentAI.InitializeObsAvoidance(new ObstacleAvoidance(transform, _target, _avoidanceParametersSo.Radius,
                _avoidanceParametersSo.ObstacleMask, _avoidanceParametersSo.AvoidWeight));
            
        }

        private void ShootState()
        {
            _fsm.Transition("Shoot");
        }
      
        private void Update()
        {   
            _fsm.OnTick();  
             
            if (Input.GetKeyDown(KeyCode.Space))
            {
               //_initTree.Execute();   
            }     

            //EnemyIsInRange();    
        }           
                           
        private void ChaseEnemy()
        {  
            _fsm.Transition("Chase");  
            //_agentAI.ChangeSteering(new PursuitSteering(transform, _target, _agentAI.GetVelocity(), 1));    
        }
        
        private void HideFromEnemy()
        {      
            _fsm.Transition("Hide");    
            _agentAI.ChangeSteering(new HideSteering(this.transform, _nearestWeapon, _target, 
                _agentAI.GetVelocity(),_waypoints));  
        }

        private void AgentIsDead() => _fsm.Transition("Dead");

        private bool EnemyIsInRange()
        {
            _agentSight.InSight = _agentSight.IsInSight(transform, _target, _agentSight.FOVRange,
                _agentSight.FOVAngle, _agentSight.FOVLayerMask);           
            return _agentSight.InSight;
        }

        private void ReloadState()
        {
            _fsm.Transition("Reload");
            StartCoroutine(ExecuteTree());
        }          

        public void Wander()
        {
            _fsm.Transition("Wander");
            StartCoroutine(ExecuteTree());
        }

        public void Patrol()
        {
            _fsm.Transition("Patrol");
        }

        [ContextMenu(nameof(MakeARandomDecision))]
        private void MakeARandomDecision()
        {
            _fsm.Transition(_roulette.Run(_randomDecision));       
        }

        IEnumerator ExecuteTree()
        {
            yield return new WaitForSeconds(2.5f);
            _initTree.Execute();
        }

        private void FindWaypoints()
        {
            GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(_agentAI.WaypointTag());

            foreach (var waypoint in waypointObjects)
            {
                _waypoints.Add(waypoint.transform);
            }
        }                   

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * _agentSight.FOVRange);
            Gizmos.DrawWireSphere(transform.position, _agentSight.FOVRange);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, _agentSight.FOVAngle / 2, 0)
                                               * transform.forward * _agentSight.FOVRange);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0, - _agentSight.FOVAngle / 2, 0)
                                               * transform.forward * _agentSight.FOVRange);
        }

        public void ExecuteTreeAgain()
        {  
            _initTree.Execute();
        }
    }
}