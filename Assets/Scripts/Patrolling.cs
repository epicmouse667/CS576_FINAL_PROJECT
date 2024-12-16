using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Patrolling : MonoBehaviour
{
    public Transform[] points; //patrol points
    // TODO: remove dummy_player and assign to player
    public Transform player; // Reference to the player

    private int current;
    private float speed = 2.0f;
    private float detectionRadius = 50.0f; // Radius to detect the player
    private LayerMask detectionLayer; // Layer for raycast detection (e.g., Player layer)
    private bool isChasing = false;
    private bool returningToPatrol = false;
    private GameObject projectile_template;
    private int levelNumber;
    // Start is called before the first frame update
    void Start()
    {
        // projectile_template = (GameObject)Resources.Load("Rock", typeof(GameObject));  // projectile prefab
        // Debug.Log(projectile_template);
        // if (projectile_template == null)
        //     Debug.LogError("Error: could not find the rock prefab in the project! Did you delete/move the prefab from your project?");
        Scene currentScene = SceneManager.GetActiveScene();
        levelNumber = currentScene.name == "level2" ? 2 : 1;
        current = 0;
        StartCoroutine("Spawn");
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
            // Debug.Log("raycast hit");
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

    private IEnumerator Spawn()
    {
        while (true)
        {
            if (isChasing)
            {
                Debug.Log("Spawning sphere...");
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // Generate a random primitive
                PrimitiveType[] primitiveTypes = { PrimitiveType.Sphere, PrimitiveType.Cube, PrimitiveType.Cylinder, PrimitiveType.Capsule };
                int rand = Random.Range(0, primitiveTypes.Length);
                GameObject primitive = GameObject.CreatePrimitive(primitiveTypes[rand]);

                // Set its position and scale
                Vector3 startPos = transform.position + 1.1f * directionToPlayer;
                startPos.y = levelNumber == 1 ? 1.0f : 4.0f;
                primitive.transform.position = startPos;
                // primitive.transform.position.y = 10.0f;
                primitive.transform.localScale = levelNumber == 1 ? new Vector3(0.2f, 0.2f, 0.2f) : new Vector3(0.5f, 0.5f, 0.5f); // Adjust scale if needed

                // Add necessary components
                Rigidbody rb = primitive.AddComponent<Rigidbody>();
                rb.useGravity = false; // Prevent gravity if it's unnecessary

                Collider collider = primitive.GetComponent<Collider>();
                collider.isTrigger = true; // Set as trigger for interaction with player

                // Set the material color to red
                Renderer renderer = primitive.GetComponent<Renderer>();
                renderer.material.color = Color.red;

                // add tag
                primitive.tag = "Obstacle";

                // Add the Rock script dynamically
                Rock rock = primitive.AddComponent<Rock>();
                rock.direction = directionToPlayer; // Assign normalized direction
                rock.velocity = 5.0f;               // Set velocity
                rock.birth_time = Time.time;        // Record spawn time

                Debug.Log("Sphere instantiated successfully.");
            }
            yield return new WaitForSeconds(2.5f); // next shot will be shot after this delay
        }
    }
}
