using AIBehaviours;
using Interfaces;
using UnityEngine; 

namespace AgentLogic
{
    public class AgentA : MonoBehaviour, IMove, IAttack
    {
       [field: SerializeField] public int Power { get; set; }
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AgentAttributes _agentAttributes;
        [SerializeField] private HealthController _healthController;
         
       

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _healthController = GetComponent<HealthController>();
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
            Debug.Log(_healthController.IsAlive());
            return _healthController.IsAlive();
        }

        public bool CheckPower()
        {
            return Power >= 5;
        }

        public void Move(Vector3 direction)
        {
            Debug.Log("WalkState");            
        }

        public void Shoot()
        {
            throw new System.NotImplementedException();
        }

        public void Pursuit()
        {
            Debug.Log("IAttack");
        }

        public void Reload()
        {
            throw new System.NotImplementedException();
        }
    }
}