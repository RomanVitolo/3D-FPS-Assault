using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform target; // El objetivo a evadir
    public float maxSpeed = 2.0f; // Velocidad máxima del objeto
    public float predictionTime = 1.0f; // Tiempo de predicción (ajusta según sea necesario)

    // Update is called once per frame
    void Update()
    {
        /*var speedX = 0.01f;   
        transform.position += new Vector3(0, 0, 5f * Time.deltaTime);   */  
        
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
