using System.Collections.Generic;
using System.Linq;         
using Interfaces;              
using UnityEngine;
using Random = UnityEngine.Random;

namespace AgentLogic
{
    public class AgentAI : MonoBehaviour, IMove, IAttack, IEntity, IPoints
    {   
        public List<Node> Waypoints;   
        public AgentWeapon AgentWeapon;
        public List<Transform>Targets = new List<Transform>();
        public Transform Target;
        [field: SerializeField] public bool CanMove { get; set;}          
        
        [SerializeField] private GameEngine _gameEngine;
        [SerializeField] private AgentInput _agentInput;
        [SerializeField] private AgentAttributes _agentAttributes;
        [SerializeField] private AgentHealth _agentHealth;
        [SerializeField] private AvoidanceParametersSO _avoidanceParameters;
        [SerializeField] private IgnoreAgentsCollisionUtilitiesSO _agentsCollision;
        [SerializeField] private CharacterController _characterController; 
        [SerializeField] private AgentAnimations _agentAnimations;

        private Collider _adjustAgentCollider;
        
        private ISteeringBehaviour _steeringBehaviour;  
        
        private float _lastShotTime = 2f;
        private int _nextPoint;    
        
        private void Awake() => InitComponents();       
        
        private void InitComponents()
        {
            _gameEngine = FindObjectOfType<GameEngine>();
            _agentInput = GetComponent<AgentInput>();
            _agentHealth = GetComponent<AgentHealth>();
            _characterController = GetComponent<CharacterController>();     
            _agentAnimations = GetComponent<AgentAnimations>();
            AgentWeapon = GetComponentInChildren<AgentWeapon>();    
        }
               
        private void Start()
        {  
            CanMove = true; 
            LoadTargets();
            Target = Targets.FirstOrDefault();
            IgnoreFriendlyCollision();
        }

        private void Update()
        {
            var center = _characterController.center;
            center.y = transform.localPosition.y;
            _characterController.center = center;
        }

        private void LoadTargets()
        {  
            foreach (var agent in _gameEngine.TeamAgentsPosition)
            {   
                if (agent.Key != _agentAttributes.TeamName)
                {
                    var assignAgentsToCorrectTeam = agent.Value;   
                    foreach (var transform in assignAgentsToCorrectTeam)
                    {
                        Targets.Add(transform);
                    }     
                }
            }  
        }         

        public void FindNearestTarget(float agentVisionDistance)
        {
            Targets.Clear();
            LoadTargets();
            Target = null;
            if (Targets != null)
            {
                foreach (var target in Targets)
                {
                    var enemyDistance = Vector3.Distance(transform.position, target.position);
                    if (enemyDistance < agentVisionDistance)
                    {      
                        Target = target;
                    }                                    
                }  
            }        
        }

        private void IgnoreFriendlyCollision()
        {
            _agentsCollision.FindColliders();   
            _agentsCollision.IgnoreCollision(_characterController);
        }

        public float GetVelocity()
        {
            var randomSpeed = Random.Range(6.5f, _agentAttributes.AgentSpeed);
            return randomSpeed;
            //return _agentAttributes.AgentSpeed;
        }         
        
        public void Dead()
        {    
             if (!_agentHealth.IsAlive())
             {
                 _agentAnimations.DeadAnimation();
                 Destroy(this.gameObject, 2.5f);
             }    
        }    
        
        public bool CheckIfPlayerIsAlive()
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
            AgentWeapon.LoadReloadFX();
            Debug.Log("Weapon Reload");       
        }     

        public void Hide()
        {        
            Move(_steeringBehaviour.GetDirection());         
            _agentAnimations.HideAnimation();

            if (_steeringBehaviour.GetDirection() == Vector3.zero && _agentInput.GetHorizontalAxis() == 0 &&
                _agentInput.GetVerticalAxis() == 0)
            {
                _agentAnimations.RunChaseAnimation();   
            } 
        }         
        
        public bool HideOrIdle()
        {
            if (CanMove) return true;
            
            return false;   
        }               

        public void Idle() => _agentAnimations.DoIdleAnimation();

      
        public void Shoot()
        {
            if (Target != null)
            {
                if (AgentWeapon.CheckForEnoughAmmo())
                {
                    _agentAnimations.ShootAnimation(true);
                    Vector3 direction = Target.position - transform.position;
                    var rotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
                        _agentAttributes.AgentTurnSpeed * Time.deltaTime);

                    if (Time.time - _lastShotTime >= 1 / AgentWeapon.WeaponFireRate())
                    {
                        AgentWeapon.Shoot();
                        _lastShotTime = Time.time;
                    }
                }      
                else
                {
                    _agentAnimations.ShootAnimation(false);
                    Idle();    
                    Debug.Log("Please Reload");  
                }  
            }
            else
            {
                _agentAnimations.ShootAnimation(false);
                Idle();    
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
            Waypoints = newPoints;
            var pos = Waypoints[_nextPoint].transform.position;
            pos.y = transform.position.y;
            transform.position = pos;  
        }
        
        public void Run()
        {
            var point = Waypoints[_nextPoint];
            var posPoint = point.transform.position;
            posPoint.y = transform.position.y;
            Vector3 dir = posPoint - transform.position;
            if (dir.magnitude < 0.2f)
            {
                if (_nextPoint + 1 < Waypoints.Count)
                    _nextPoint++;
                else
                {   
                    IdleState();     
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
        }     
    }         
}