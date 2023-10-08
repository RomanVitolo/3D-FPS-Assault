using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float hideThreshold = 0.5f; // 50% health threshold 
   
    public float moveSpeed = 2.0f;

    public List<Transform> waypoints; // Lista de waypoints en la escena

    private Transform nearestWaypoint;

    private void Start()
    {
        // Buscar el waypoint más cercano al inicio
        FindNearestWaypoint();
    }

    private void Update()
    {   
         
            if (nearestWaypoint != null)
            {
                Vector3 steeringForce = SteerTowardsWaypoint(nearestWaypoint.position);
                Move(steeringForce);
            }             
    }

    public void FindNearestWaypoint()
    {
        float minDistance = float.MaxValue;

        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector3.Distance(transform.position, waypoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWaypoint = waypoint;
            }
        }
    }

    private Vector3 SteerTowardsWaypoint(Vector3 waypointPosition)
    {
        Vector3 desiredDirection = waypointPosition - transform.position;
        desiredDirection.Normalize();
        Vector3 desiredVelocity = desiredDirection * moveSpeed;
        Vector3 steeringForce = desiredVelocity - transform.forward;
        return steeringForce;
    }

    private void Move(Vector3 moveDirection)
    {
        // Aplicar movimiento al enemigo
        transform.Translate(moveDirection * Time.deltaTime);
    }
}