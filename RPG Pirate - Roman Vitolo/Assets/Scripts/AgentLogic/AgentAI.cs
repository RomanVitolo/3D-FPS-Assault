using System;
using Interfaces;
using UnityEngine;   

namespace AgentLogic
{
    public class AgentAI : MonoBehaviour, IMove, IAttack, IEntity
    {            
        [SerializeField] private AgentAttributes _agentAttributes;
        [SerializeField] private AgentHealth _agentHealth; 
        
        [SerializeField] private CharacterController _characterController; 
        [SerializeField] private AgentAnimations _agentAnimations;
        
        [SerializeField] private AgentWeapon _agentWeapon;
        [SerializeField] private Transform _target;
                               
        private ISteeringBehaviour _steeringBehaviour;
        private ISteeringBehaviour _obsAvoidance;

        private float _lastShotTime = 1f;
        
        private void Awake()
        { 
            _agentHealth = GetComponent<AgentHealth>();
            _characterController = GetComponent<CharacterController>();
            _agentAnimations = GetComponent<AgentAnimations>();
            _agentWeapon = GetComponentInChildren<AgentWeapon>();
        }

        private void Start()
        {
            _agentAttributes.InitializeWeapon();
        }       
        
        public float GetVelocity()
        {
            return _agentAttributes.AgentSpeed;
        }         
        
        public void Dead()
        {    
             if (_agentHealth.IsAlive() == false)
             {
                 _agentAnimations.DeadAnimation();
                 Destroy(this.gameObject, 2.5f);
             }
             
             Debug.Log("Agent dead");
        }    

        public void Spin()
        {
            Debug.Log("Spin");
        }
        
        public bool CheckLife()
        {         
            Debug.Log(_agentHealth.IsAlive());
            return _agentHealth.IsAlive();
        }

        public bool CheckLowLife()
        {     
            if (_agentHealth.GetCurrentLife() < 50 && _agentHealth.GetCurrentLife() > 0)
            {
                return true;
            }            
            return false;
        }         

        public void Move(Vector3 direction)
        { 
            direction.y = 0;
            var setDirection = direction * (GetVelocity() * Time.deltaTime); 
            Quaternion rotation = Quaternion.LookRotation(setDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _agentAttributes.AgentTurnSpeed * Time.deltaTime);  
            _characterController.Move(setDirection);    
        }     
          
        public void Pursuit() => Move(_obsAvoidance.GetDirection());    

        public void Idle()
        {
            _agentAnimations.DoIdleAnimation();
        }

        public void Reload(bool reload)
        {
            _agentAnimations.ReloadAnimation(reload);  
            Debug.Log("Weapon Reload");       
        }

        public void Hide()
        {
            Move(_steeringBehaviour.GetDirection()); 
            Debug.Log("Hide Action");
        }    
            
        public void Shoot()
        {
            if (_target == null)
            {
                _agentAnimations.ShootAnimation(false);
                return;
            } 
            
            _agentAnimations.ShootAnimation(true);
            Vector3 direction = _target.position - transform.position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _agentAttributes.AgentTurnSpeed * Time.deltaTime);  
                  
            if (Time.time - _lastShotTime >= 1 / _agentWeapon.WeaponFireRate())
            {   
                _agentWeapon.Shoot();  
                _lastShotTime = Time.time;
            }     
              
        }      

        public void SwitchWeapon()
        {  
            Debug.Log("Switch Weapon");
            _agentAttributes.WeaponGO[0].SetActive(false);
            _agentAttributes.WeaponGO[1].SetActive(true);  
        }
        

        public void ChangeSteering(ISteeringBehaviour steeringBehaviour) => _steeringBehaviour = steeringBehaviour;  
        public void InitializeObsAvoidance(ISteeringBehaviour obstacleAvoidance) => _obsAvoidance = obstacleAvoidance;
     }  
   
}