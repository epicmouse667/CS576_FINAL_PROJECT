using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    public Transform[] points; //patrol points
    public Transform player; // Reference to the player

    private int current;
    private float speed = 2.0f;
    private float detectionRadius = 10.0f; // Radius to detect the player
    private LayerMask detectionLayer; // Layer for raycast detection (e.g., Player layer)
    private bool isChasing = false;
    private bool returningToPatrol = false;
    // Start is called before the first frame update
    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInSight())
        {
            ChasePlayer();
        }
        else if (isChasing)
        {
            // Player escaped, return to nearest patrol point
            isChasing = false;
            returningToPatrol = true;
            current = GetClosestPatrolPointIndex();
        }
        else if (returningToPatrol)
        {
            ReturnToPatrolPoint();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        returningToPatrol = false;

        if (transform.position != points[current].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
            RotateTowards(points[current].position); // Rotate towards the patrol point
        }
        else
        {
            current = (current + 1) % points.Length;
        }
    }

    void ChasePlayer()
    {
        isChasing = true;

        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        RotateTowards(player.position); // Rotate towards the player
    }

    void ReturnToPatrolPoint()
    {
        if (transform.position != points[current].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
            RotateTowards(points[current].position); // Rotate towards the patrol point
        }
        else
        {
            returningToPatrol = false; // Reached patrol point
        }
    }

    void RotateTowards(Vector3 targetPosition)
    {
        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate the rotation angle
        float angle_to_rotate = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Euler(0.0f, angle_to_rotate, 0.0f);
    }

    bool IsPlayerInSight()
    {
        // Debug.Log(player.position);
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Perform a raycast to check if the player is in sight
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRadius))
        {
            Debug.Log("raycast hit");
            // Check if the raycast hit the player
            return hit.transform == player;
        }

        return false;
    }

    int GetClosestPatrolPointIndex()
    {
        float closestDistance = Mathf.Infinity;
        int closestPointIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, points[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPointIndex = i;
            }
        }

        return closestPointIndex;
    }
}
