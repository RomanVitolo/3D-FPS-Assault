using AIBehaviours;
using DecisionTree;
using FSM;
using Interfaces;
using UnityEngine; 

namespace AgentLogic
{
    public class AgentAController : MonoBehaviour
    {
        [SerializeField] private AgentA _agentA;
        [SerializeField] private AgentInput _agentInput;

        private INode _initTree;
        private FSM<string> _fsm;
        private Rigidbody _rb;
        private ISteeringBehaviour _steeringBehaviour;
        private bool _doTransition;

        private void Awake()
        {
            _agentA = GetComponent<AgentA>();
            _agentInput = GetComponent<AgentInput>();
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {  
            _fsm = new FSM<string>();
            
            IdleState<string> idleState = new IdleState<string>();
            PatrolState<string> patrolState = new PatrolState<string>(_agentA);
            ChaseState<string> chaseState = new ChaseState<string>(_agentA);
           
            _fsm.InitializeFSM(idleState);
            
            
            idleState.AddTransition("ChaseState", chaseState);   
            chaseState.AddTransition("Idle", idleState);
            
            ActionNode dead = new ActionNode(_agentA.Dead);
            ActionNode getPower = new ActionNode(_agentA.GetPower);
            ActionNode spin = new ActionNode(_agentA.Spin);

            QuestionNode hasPower = new QuestionNode(_agentA.CheckPower, idleState, chaseState);
            QuestionNode hasLife = new QuestionNode(_agentA.CheckLife, chaseState, idleState);  

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
                _doTransition = true;
                ChaseEnemy(); 
                //_fsm.Transition("Walk");
            }    
        } 
        
        private void ChangeSteering(ISteeringBehaviour steeringBehaviour)
        {
            _steeringBehaviour = steeringBehaviour;
        }

        private void CheckForChasing()
        {
            _initTree.Execute();
        }

        private void ChaseEnemy()
        {
            if (_doTransition)
            {
                _initTree.Execute();
                _fsm.Transition("ChaseState");
                ChangeSteering(new PursuitSteering());
                _doTransition = false;
            }
            _steeringBehaviour.GetDirection();
            _agentA.Pursuit();
        }

    }
}