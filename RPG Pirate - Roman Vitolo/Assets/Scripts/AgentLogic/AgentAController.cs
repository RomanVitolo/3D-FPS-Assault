using System.Collections.Generic;
using AIBehaviors;
using AIBehaviours;
using DecisionTree;
using FSM;
using LineOfSight;
using UnityEngine;           

namespace AgentLogic
{
    public class AgentAController : MonoBehaviour
    {
        [SerializeField] private AgentAI _agentAI;
        [SerializeField] private AgentInput _agentInput;
        [SerializeField] private Transform _target;
        [SerializeField] private LineOfSightConfigurationSO _agentSight;
        [SerializeField] private AvoidanceParameters _avoidanceParameters;
        [SerializeField] private List<Transform> _waypoints = new List<Transform>();

        private INode _initTree;
        private FSM<string> _fsm;
        private Rigidbody _rb;
        private Transform _nearestWeapon;

        private void Awake()
        {
            _agentAI = GetComponent<AgentAI>();
            _agentInput = GetComponent<AgentInput>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {  
            _fsm = new FSM<string>();

            DeadState<string> deadState = new DeadState<string>(_agentAI);
            IdleState<string> idleState = new IdleState<string>(_agentAI);
            PatrolState<string> patrolState = new PatrolState<string>(_agentAI);
            ChaseState<string> chaseState = new ChaseState<string>(_agentAI);
            HideState<string> hideState = new HideState<string>(_agentAI);
            ReloadState<string> reloadState = new ReloadState<string>(_agentAI);

            _fsm.InitializeFSM(idleState);
            
            idleState.AddTransition("Chase", chaseState);  
            idleState.AddTransition("Hide", hideState);  
            idleState.AddTransition("Dead", deadState);  
            idleState.AddTransition("Reload", reloadState);  
            
            chaseState.AddTransition("Idle", idleState);
            chaseState.AddTransition("Hide", hideState);
            chaseState.AddTransition("Dead", deadState); 
            
            hideState.AddTransition("Chase", chaseState);
            hideState.AddTransition("Dead", deadState); 
            
            reloadState.AddTransition("Idle", idleState);
            
            ActionNode dead = new ActionNode(AgentIsDead); 
            ActionNode spin = new ActionNode(ChaseEnemy);
            ActionNode hide = new ActionNode(HideFromEnemy);
            ActionNode canAttack = new ActionNode(HideFromEnemy);
            ActionNode Patrol = new ActionNode(HideFromEnemy);           
            ActionNode reload = new ActionNode(ReloadState);           
            ActionNode idle = new ActionNode(IdleState);           
            
            QuestionNode isInRange = new QuestionNode(EnemyIsInRange, canAttack, Patrol);
            QuestionNode dieOrHide = new QuestionNode(_agentAI.CheckLowLife, hide, isInRange);
            QuestionNode hasLife = new QuestionNode(_agentAI.CheckLife, reload, idle);  

            _initTree = hasLife;
            _initTree.Execute();
            
            _agentAI.InitializeObsAvoidance(new ObstacleAvoidance(transform, _target, _avoidanceParameters.Radius,
                _avoidanceParameters.ObstacleMask, _avoidanceParameters.AvoidWeight));
        }   
      
        private void Update()
        {   
             _fsm.OnTick();

             if (_agentInput.GetHorizontalAxis() != 0 || _agentInput.GetVerticalAxis() != 0)
             {
               _fsm.Transition("Walk");  
             }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _initTree.Execute();  
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
        }

        private void IdleState()
        {
            _fsm.Transition("Idle");
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
    }
}