using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public Transform player;
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
        // Comprobar la distancia entre el enemigo y el jugador
        float playerDistance = Vector3.Distance(transform.position, player.position);

        // Si la distancia al jugador es mayor que el umbral (en este caso, no hay umbral), dirigirse hacia el waypoint más cercano que no esté cerca del jugador.
        if (playerDistance > 2.0f) // Puedes ajustar la distancia deseada
        {
            if (nearestWaypoint != null)
            {
                Vector3 steeringForce = SteerTowardsWaypoint(nearestWaypoint.position);
                Move(steeringForce);
            }
        }
    }

    private void FindNearestWaypoint()
    {
        float minDistance = float.MaxValue;

        foreach (Transform waypoint in waypoints)
        {
            // Calcular la distancia entre el enemigo y el waypoint
            float distance = Vector3.Distance(transform.position, waypoint.position);

            // Calcular la distancia entre el enemigo y el jugador
            float playerDistance = Vector3.Distance(transform.position, player.position);

            // Si la distancia al jugador es mayor que el umbral (en este caso, no hay umbral) y la distancia al waypoint es menor que la distancia al jugador,
            // actualizar el waypoint más cercano.
            if (playerDistance > 2.0f && distance < minDistance) // Puedes ajustar la distancia deseada
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