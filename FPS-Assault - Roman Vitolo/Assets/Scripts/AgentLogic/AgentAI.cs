using System.Collections.Generic;
using Interfaces;
using UnityEngine;   

namespace AgentLogic
{
    public class AgentAI : MonoBehaviour, IMove, IAttack, IEntity, IPoints
    {        
        public List<Node> waypoints;
        public bool readyToMove;                
        [field: SerializeField] public bool CanMove { get; set;}
        [field: SerializeField] public bool NewQuestion { get; set;}

        [SerializeField] private AgentInput _agentInput;
        [SerializeField] private AgentAttributes _agentAttributes;
        [SerializeField] private AgentHealth _agentHealth; 
        [SerializeField] private AgentAController _agentAController;  
        [SerializeField] private AvoidanceParametersSO _avoidanceParameters;  
        
        [SerializeField] private CharacterController _characterController; 
        [SerializeField] private AgentAnimations _agentAnimations;
        
        [SerializeField] private AgentWeapon _agentWeapon;
        [SerializeField] private Transform _target;        
                               
        private ISteeringBehaviour _steeringBehaviour;  
        
        private float _lastShotTime = 1f;
        private int _nextPoint;
        
        private void Awake()
        {                
            _agentInput = GetComponent<AgentInput>();
            _agentHealth = GetComponent<AgentHealth>();
            _characterController = GetComponent<CharacterController>();
            _agentAController = GetComponent<AgentAController>();
            _agentAnimations = GetComponent<AgentAnimations>();
            _agentWeapon = GetComponentInChildren<AgentWeapon>();   
        }

        private void Start()
        {  
            CanMove = true;
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
            SimpleAvoidance();
            direction.y = 0;
            var setDirection = direction * (GetVelocity() * Time.deltaTime); 
            Quaternion rotation = Quaternion.LookRotation(setDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _agentAttributes.AgentTurnSpeed * Time.deltaTime);     
            _characterController.Move(setDirection);              
        }     
          
        public void Pursuit() => Move(_steeringBehaviour.GetDirection());         

        public void Reload(bool reload)
        {
            _agentAnimations.ReloadAnimation(reload); 
            _agentWeapon.LoadReloadFX();
            Debug.Log("Weapon Reload");       
        }

        public void Hide()
        {        
            Move(_steeringBehaviour.GetDirection());   
            Debug.Log("Hide Action"); 
            _agentAnimations.HideAnimation();

            if (_steeringBehaviour.GetDirection() == Vector3.zero && _agentInput.GetHorizontalAxis() == 0 &&
                _agentInput.GetVerticalAxis() == 0)
            {
                _agentAnimations.DoIdleAnimation();
                NewQuestion = true;
                
                if (NewQuestion)
                {
                    Debug.Log("Doing Idle");
                    NewQuestion = false;
                    CanMove = false;
                    _agentAController.ExecuteTreeAgain();
                } 
            } 
        }         
        
        public bool HideOrWander()
        {
            if (CanMove) return true;
            
            return false;   
        }               

        public void Wander() => _agentAnimations.DoIdleAnimation();
      
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
                 
        public void ChangeSteering(ISteeringBehaviour steeringBehaviour) => _steeringBehaviour = steeringBehaviour; 
        
        public string WaypointTag() => _agentAttributes.WaypointNameTag;
                                                                                 
        private void SimpleAvoidance()
        {
            Vector3 direction = transform.forward;      
            
            Vector3 actualDirection = transform.position + direction * _avoidanceParameters.Radius;   
            
            Collider[] obstacle = Physics.OverlapSphere(actualDirection, _avoidanceParameters.AvoidWeight, _avoidanceParameters.ObstacleMask);

            if (obstacle.Length > 0)
            {          
                Vector3 evasion = Vector3.Cross(Vector3.up, direction);
                direction += evasion;
                                               
                direction += Quaternion.Euler(1, 45, 1) * evasion;
            }                              
            transform.position += direction.normalized * (_agentAttributes.AgentSpeed * Time.deltaTime);      
        }                                     
        
        public void SetWayPoints(List<Node> newPoints)
        {
            _nextPoint = 0;
            if (newPoints.Count == 0) return;   
            waypoints = newPoints;
            var pos = waypoints[_nextPoint].transform.position;
            pos.y = transform.position.y;
            transform.position = pos;
            readyToMove = true;
        }
        
        public void Run()
        {
            var point = waypoints[_nextPoint];
            var posPoint = point.transform.position;
            posPoint.y = transform.position.y;
            Vector3 dir = posPoint - transform.position;
            if (dir.magnitude < 0.2f)
            {
                if (_nextPoint + 1 < waypoints.Count)
                    _nextPoint++;
                else
                {   
                    IdleState();   
                    readyToMove = false;   
                    return;
                }
            }       
            SimpleAvoidance();
            Move(dir.normalized);
            _agentAnimations.RunChaseAnimation();
        }

        private void IdleState()
        {
            _agentAnimations.DoIdleAnimation();
            NewQuestion = true;
                
            if (NewQuestion)
            {
                Debug.Log("Doing Idle");
                NewQuestion = false;      
                _agentAController.ExecuteTreeAgain();
            }         
        }
        
    }         
}