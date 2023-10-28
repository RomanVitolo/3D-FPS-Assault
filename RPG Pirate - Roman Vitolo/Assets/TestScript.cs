using System;
using DefaultNamespace;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform target; 
    public float maxSpeed = 2.0f; 
    public float predictionTime = 1.0f; 
    
    void Update()
    {
        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;
        float speed = maxSpeed;

        float prediction;
        if (speed <= distance / predictionTime)
        {
            prediction = predictionTime;
           
        }
        else
        {
            prediction = distance / speed;  
        }      
        
        Vector3 predictedTarget = target.position + target.forward * prediction;

        Vector3 desiredVelocity = (predictedTarget - transform.position).normalized * maxSpeed;
        Vector3 steering = desiredVelocity - transform.forward;

        transform.position += transform.forward * (maxSpeed * Time.deltaTime);
        transform.forward = steering.normalized;
        
    }    
}
