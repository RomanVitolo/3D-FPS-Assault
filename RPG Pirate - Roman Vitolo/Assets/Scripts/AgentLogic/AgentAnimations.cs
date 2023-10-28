using System.Collections;
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
           _agentAnimator.SetFloat("AgentSpeed", 1);  
        }

        public void DoIdleAnimation()
        {
            _agentAnimator.SetBool("Reload", false);
        }

        public void ReloadAnimation(bool setAnimation)
        {
            _agentAnimator.SetBool("Reload", setAnimation);
        }
        
        public void ShootAnimation(bool shoot)
        {
              _agentAnimator.SetBool("Shoot", shoot);
        }

        public void DeadAnimation()
        {
            _agentAnimator.SetBool("Dead", true);
            StartCoroutine(DestroyAgent());
        }

        IEnumerator DestroyAgent()
        { 
            yield return new WaitForSeconds(2.5f);
            _agentAnimator.SetBool("Dead", false);     
        }   
        
    }
}