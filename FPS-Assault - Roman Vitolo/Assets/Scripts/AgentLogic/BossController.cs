﻿using System.Collections;
using System.Collections.Generic;
using AIBehaviors;  
using DecisionTree;
using DefaultNamespace;
using FSM;
using Interfaces;
using LineOfSight;
using UnityEngine;     

namespace AgentLogic
{
    public class BossController: MonoBehaviour, IReady
    {
        public bool InSight;
        public List<Transform> Waypoints;
        
        [SerializeField] private AgentAI _agentAI;
        [SerializeField] private LineOfSightConfigurationSO _agentSight;
        [SerializeField] private AvoidanceParametersSO _avoidanceParametersSo;
        [SerializeField] private AgentPathfindingConfig _agentPath;            

        private INode _initTree;
        private FSM<string> _fsm;  
        private Transform _nearestWeapon;
        private Roulette _roulette;
        private Dictionary<string, int> _randomDecision = new Dictionary<string, int>();     

        private void Awake()
        {
            _agentAI = GetComponent<AgentAI>();
            _agentPath = GetComponent<AgentPathfindingConfig>(); 
        }          

        private void Start()
        {
            CanDoANewQuestion(false);
            InSight = false;
            FindWaypoints();
            
            _roulette = new Roulette();  
            _randomDecision.Add("Shoot", 20);
            _randomDecision.Add("Chase", 70);    
            _randomDecision.Add("Patrol", 10);  
            
            _fsm = new FSM<string>();

            DeadState<string> deadState = new DeadState<string>(_agentAI);
            IdleState<string> idleState = new IdleState<string>(_agentAI);
            PatrolState<string> patrolState = new PatrolState<string>(_agentAI, _agentPath);
            ChaseState<string> chaseState = new ChaseState<string>(_agentAI);
            HideState<string> hideState = new HideState<string>(_agentAI, _agentAI.CanMove);
            ReloadState<string> reloadState = new ReloadState<string>(_agentAI);
            ShootState<string> shootState = new ShootState<string>(_agentAI);    

            _fsm.InitializeFSM(idleState);
            
            idleState.AddTransition("Chase", chaseState);  
            idleState.AddTransition("Hide", hideState);  
            idleState.AddTransition("Dead", deadState);  
            idleState.AddTransition("Reload", reloadState);  
            idleState.AddTransition("Shoot", shootState);  
            idleState.AddTransition("Patrol", patrolState); 
            
            patrolState.AddTransition("Idle", idleState);
            patrolState.AddTransition("Hide", hideState);   
            patrolState.AddTransition("Shoot", shootState);   
            
            chaseState.AddTransition("Idle", idleState);
            chaseState.AddTransition("Hide", hideState);
            chaseState.AddTransition("Dead", deadState); 
            chaseState.AddTransition("Shoot", shootState); 
            
            hideState.AddTransition("Idle", idleState);
            hideState.AddTransition("Chase", chaseState);
            hideState.AddTransition("Dead", deadState); 
            hideState.AddTransition("Patrol", patrolState); 
            hideState.AddTransition("Shoot", shootState); 
            
            reloadState.AddTransition("Idle", idleState);
            reloadState.AddTransition("Shoot", shootState);
            reloadState.AddTransition("Dead", deadState);
            
            shootState.AddTransition("Idle", idleState);
            shootState.AddTransition("Chase", chaseState);                  
            shootState.AddTransition("Reload", reloadState);                  
            shootState.AddTransition("Dead", deadState);                  
            
            ActionNode dead = new ActionNode(AgentIsDead); 
            ActionNode chase = new ActionNode(ChaseEnemy);
            ActionNode hide = new ActionNode(HideFromEnemy);
            ActionNode attack = new ActionNode(ShootState);
            ActionNode patrol = new ActionNode(Patrol);           
            ActionNode reload = new ActionNode(ReloadState);           
            ActionNode idle = new ActionNode(DoIdle);          
                                                         
            QuestionNode hasAmmo = new QuestionNode(_agentAI.AgentWeapon.CheckForEnoughAmmo, attack, reload);
            QuestionNode canChase = new QuestionNode(EnemyIsInRange, hasAmmo, idle);
            QuestionNode isInRange = new QuestionNode(EnemyIsInRange, canChase, idle);
            QuestionNode canWander = new QuestionNode(EnemyIsInRange, isInRange, patrol);
            QuestionNode hidePath = new QuestionNode(_agentAI.HideOrIdle, hide, canWander);
            QuestionNode hasLowLife = new QuestionNode(_agentAI.CheckLowLife, hidePath, canWander);
            QuestionNode hasLife = new QuestionNode(_agentAI.CheckIfPlayerIsAlive, hasLowLife, dead);  

            _initTree = hasLife;
            _initTree.Execute();            
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
               _initTree.Execute();   
            }         
        }           
                           
        private void ChaseEnemy()
        {  
            _fsm.Transition("Chase");     
        }
        
        private void HideFromEnemy()
        {    
            _agentAI.FindNearestTarget(_agentSight.FOVRange);  
            _fsm.Transition("Hide");    
            _agentAI.ChangeSteering(new HideSteering(this.transform, _nearestWeapon, _agentAI.Target, 
                _agentAI.GetVelocity(),Waypoints));  
        }

        private void AgentIsDead() => _fsm.Transition("Dead");

        private bool EnemyIsInRange()
        {
            _agentAI.FindNearestTarget(_agentSight.FOVRange);

            if (_agentAI.Target == null)
            {
                DoIdle();
                return false;
            }
            InSight = _agentSight.IsInSight(transform, _agentAI.Target, _agentSight.FOVRange,
                _agentSight.FOVAngle, _agentSight.FOVLayerMask);           
            return InSight;
        }

        private void ReloadState()
        {
            _fsm.Transition("Reload");  
        }          

        private void DoIdle()
        {
            _fsm.Transition("Idle");       
        }

        private void Patrol()   
        {                                        
            _fsm.Transition("Patrol"); 
        }

        [ContextMenu(nameof(MakeARandomDecision))]
        private void MakeARandomDecision()
        {
            _fsm.Transition(_roulette.ExecuteRoulette(_randomDecision));       
        }
        
        public void ExecuteTreeAgain()
        {
            StartCoroutine(WaitForNewQuestion());
        }

        IEnumerator WaitForNewQuestion()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            _initTree.Execute();
            Debug.Log("New Question");
        }

        private void FindWaypoints()
        {
            GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag(_agentAI.WaypointTag());

            foreach (var waypoint in waypointObjects)
            {
                Waypoints.Add(waypoint.transform);
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

        public bool CanDoANewQuestion(bool agentReady)
        {
            return agentReady;
        }
        
    }
}