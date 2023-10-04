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
        [SerializeField] private Agent _agent;
        [SerializeField] private AgentInput _agentInput;
        [SerializeField] private Transform _target;
        [SerializeField] private LineOfSightConfigurationSO _agentSight;

        private INode _initTree;
        private FSM<string> _fsm;
        private Rigidbody _rb;     

        private QuestionNode hasLife;
        private QuestionNode isInRange;
        private QuestionNode dieOrHide;

        private void Awake()
        {
            _agent = GetComponent<Agent>();
            _agentInput = GetComponent<AgentInput>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {  
            _fsm = new FSM<string>();

            DeadState<string> deadState = new DeadState<string>(_agent);
            IdleState<string> idleState = new IdleState<string>();
            PatrolState<string> patrolState = new PatrolState<string>(_agent);
            ChaseState<string> chaseState = new ChaseState<string>(_agent);
            HideState<string> hideState = new HideState<string>(_agent);

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
            ActionNode isEnemyInRange = new ActionNode(EnemyIsInRange);
            ActionNode spin = new ActionNode(ChaseEnemy);
            ActionNode hide = new ActionNode(HideFromEnemy);

            isInRange = new QuestionNode(CheckForChasing, spin, isEnemyInRange);
            dieOrHide = new QuestionNode(_agent.CheckLowLife, hide, isInRange);
            hasLife = new QuestionNode(_agent.CheckLife, spin, dead);  

            _initTree = hasLife;
            _initTree.Execute();
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

            EnemyIsInRange();    
        }         

        private bool CheckForChasing()
        {  
            if (_agent.CheckLife())
            {
                Debug.Log("Algo");
                return true;
                
            } 
            Debug.Log("Falso");
            return false;
        }

        private void ChaseEnemy()
        {  
            _fsm.Transition("Chase");
            _agent.ChangeSteering(new PursuitSteering(transform, _target, _agent.GetVelocity(), 1));    
        }
        
        private void HideFromEnemy()
        { 
            _fsm.Transition("Hide");
            _agent.ChangeSteering(new HideSteering());  
        }

        private void AgentIsDead() => _fsm.Transition("Dead");

        private void EnemyIsInRange()
        {
            _agentSight.InSight = _agentSight.IsInSight(transform, _target, _agentSight.FOVRange,
                _agentSight.FOVAngle, _agentSight.FOVLayerMask);
        }
    }
}