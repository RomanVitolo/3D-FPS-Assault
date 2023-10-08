using UnityEngine;

namespace AgentLogic
{
    public class AgentAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _agentAnimator;

        private void Awake()
        {
            _agentAnimator = GetComponent<Animator>();
        }

        public void RunChaseAnimation()
        {
            
        }

        public void DoIdleAnimation()
        {
            //_agentAnimator.SetFloat("Idle");
        }
        
        
    }
}