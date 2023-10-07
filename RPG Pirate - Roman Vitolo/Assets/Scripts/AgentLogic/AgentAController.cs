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

        private INode _initTree;
        private FSM<string> _fsm;
        private Rigidbody _rb;      

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
            IdleState<string> idleState = new IdleState<string>();
            PatrolState<string> patrolState = new PatrolState<string>(_agentAI);
            ChaseState<string> chaseState = new ChaseState<string>(_agentAI);
            HideState<string> hideState = new HideState<string>(_agentAI);

            _fsm.InitializeFSM(idleState);
            
            idleState.AddTransition("Chase", chaseState);  
            idleState.AddTransition("Hide", hideState);  
            idleState.AddTransition("Dead", deadState);  
            
            chaseState.AddTransition("Idle", idleState);
            chaseState.AddTransition("Hide", hideState);
            chaseState.AddTransition("Dead", deadState); 
            
            hideState.AddTransition("Chase", chaseState);
            hideState.AddTransition("Dead", deadState);      
            
            ActionNode dead = new ActionNode(AgentIsDead); 
            ActionNode spin = new ActionNode(ChaseEnemy);
            ActionNode hide = new ActionNode(HideFromEnemy);
            ActionNode canAttack = new ActionNode(HideFromEnemy);
            ActionNode Patrol = new ActionNode(HideFromEnemy);           
            
            QuestionNode isInRange = new QuestionNode(EnemyIsInRange, canAttack, Patrol);
            QuestionNode dieOrHide = new QuestionNode(_agentAI.CheckLowLife, hide, isInRange);
            QuestionNode hasLife = new QuestionNode(_agentAI.CheckLife, spin, dead);  

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
            _agentAI.ChangeSteering(new PursuitSteering(transform, _target, _agentAI.GetVelocity(), 1));    
        }
        
        private void HideFromEnemy()
        { 
            _fsm.Transition("Hide");
            _agentAI.ChangeSteering(new HideSteering());  
        }

        private void AgentIsDead() => _fsm.Transition("Dead");

        private bool EnemyIsInRange()
        {
            _agentSight.InSight = _agentSight.IsInSight(transform, _target, _agentSight.FOVRange,
                _agentSight.FOVAngle, _agentSight.FOVLayerMask);           
            return _agentSight.InSight;
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