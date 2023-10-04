using UnityEngine;

namespace AgentLogic
{
    public class AgentInput : MonoBehaviour
    {  
        private float horizontalInput { get; set; }
        private float verticalInput { get; set; }

        private void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical"); 
        }

        public float GetHorizontalAxis() => horizontalInput;  
        public float GetVerticalAxis() => verticalInput;
    }
}