using AIBehaviors;
using AIBehaviours;
using DecisionTree;
using FSM;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace AgentLogic
{
    public class AgentAController : MonoBehaviour
    {
        [SerializeField] private Agent _agent;
        [SerializeField] private AgentInput _agentInput;

        private INode _initTree;
        private FSM<string> _fsm;
        private Rigidbody _rb;
        private ISteeringBehaviour _steeringBehaviour;  

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
            
            IdleState<string> idleState = new IdleState<string>();
            PatrolState<string> patrolState = new PatrolState<string>(_agent);
            ChaseState<string> chaseState = new ChaseState<string>(_agent);
            HideState<string> hideState = new HideState<string>(_agent);

            _fsm.InitializeFSM(idleState);
            
            idleState.AddTransition("Chase", chaseState);  
            
            chaseState.AddTransition("Idle", idleState);
            chaseState.AddTransition("Hide", hideState);
            
            hideState.AddTransition("Chase", chaseState);
            
            
            ActionNode dead = new ActionNode(ChaseEnemy);
            ActionNode isEnemyInRange = new ActionNode(_agent.GetPower);
            ActionNode spin = new ActionNode(_agent.Spin);
            ActionNode hide = new ActionNode(HideFromEnemy);

            isInRange = new QuestionNode(_agent.CheckPower, spin, isEnemyInRange);
            dieOrHide = new QuestionNode(_agent.CheckLowLife, hide, isInRange);
            hasLife = new QuestionNode(_agent.CheckLife, dieOrHide, dead);  

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
                //_agent.DoTransition = true;
                //_fsm.Transition("Walk");
            }    
        } 
        
        private void ChangeSteering(ISteeringBehaviour steeringBehaviour)
        {
            _steeringBehaviour = steeringBehaviour;
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
            if (_agent.DoTransition)
            {   
                _fsm.Transition("Chase");
                ChangeSteering(new PursuitSteering());
                _agent.DoTransition = false;
            }
            _steeringBehaviour.GetDirection();
            _agent.Pursuit();
        }
        
        private void HideFromEnemy()
        {
            if (_agent.DoTransition)
            {   
                _fsm.Transition("Hide");
                ChangeSteering(new HideSteering());
                _agent.DoTransition = false;
            }
            _steeringBehaviour.GetDirection();
            _agent.Hide();
        }

    }
}