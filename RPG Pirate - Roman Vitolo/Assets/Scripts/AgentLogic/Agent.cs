using AIBehaviours;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace AgentLogic
{
    public class Agent : MonoBehaviour, IMove, IAttack
    {
        [field: SerializeField] public int Power { get; set; }
        [field: SerializeField] public bool DoTransition { get; set; }
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AgentAttributes _agentAttributes;
        [SerializeField] private AgentHealth _agentHealth; 

        private ISteeringBehaviour _steeringBehaviour;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _agentHealth = GetComponent<AgentHealth>();
        }

        public void ChangeSteering(ISteeringBehaviour steeringBehaviour)
        {
            _steeringBehaviour = steeringBehaviour;
        }
        
        public void Dead()
        {
             Debug.Log("Agent dead");
        } 

        public void GetPower()
        {
            Debug.Log("Get Power");
            Power += 10;
        }

        public void Spin()
        {
            Debug.Log("Spin");
        }
        
        public bool CheckLife()
        {
            DoTransition = true;
            Debug.Log(_agentHealth.IsAlive());
            return _agentHealth.IsAlive();
        }

        public bool CheckLowLife()
        {
            DoTransition = true;
            if (_agentHealth.GetCurrentLife() < 50 && _agentHealth.GetCurrentLife() > 0)
            {
                return true;
            }            
            return false;
        }

        public bool CheckPower()
        {
            return Power >= 5;
        }

        public void Move()
        {  
            Debug.Log("Moving");            
        }

        public void Shoot()
        {
            throw new System.NotImplementedException();
        }

        public void Pursuit()
        {
            _steeringBehaviour.GetDirection();
            Debug.Log("IAttack");
        }

        public void Reload()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        { 
            _steeringBehaviour.GetDirection();
           Debug.Log("Hide Action");
        }
    }
}