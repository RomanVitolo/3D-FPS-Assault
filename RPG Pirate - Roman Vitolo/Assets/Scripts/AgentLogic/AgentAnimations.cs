using UnityEngine;

namespace AgentLogic
{
    public class AgentAnimations : MonoBehaviour
    {
        [SerializeField] private Animator AgentAnimator;

        private void Awake()
        {
            AgentAnimator = GetComponent<Animator>();
        }

        public void RunChaseAnimation()
        {
            
        }
        
        
    }
}