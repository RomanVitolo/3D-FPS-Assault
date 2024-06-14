using UnityEngine;   

namespace AgentLogic
{
    public class AgentCollider : MonoBehaviour
    {
        [SerializeField] private AgentController _AgentController;
        [SerializeField] private AgentAI _agentAI;

        private void Awake()
        {
            _agentAI = GetComponent<AgentAI>();
            _AgentController = GetComponent<AgentController>();
        }        

        private void OnTriggerEnter(Collider other)
        {
            NotifyPlayerHealth(other);
        }

        private void NotifyPlayerHealth(Collider other)
        {
            if (other == null) return;
            PlayerIsDead(); 
            CheckPlayerLife();
        }

        private void CheckPlayerLife()
        {
            if (_agentAI.CheckLowLife())
            {
                _agentAI.CanMove = true;
                ExecuteNewQuestion();
            }
        }

        private void PlayerIsDead()
        {
            if (!_agentAI.CheckIfPlayerIsAlive())
            {
                ExecuteNewQuestion();
            }
        }

        private void ExecuteNewQuestion()
        {
            Debug.Log("New Tree Question");
            _AgentController.ExecuteTreeAgain();
        }
    }
}