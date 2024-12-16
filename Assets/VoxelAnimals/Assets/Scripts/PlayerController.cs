using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3f;
    public float directionChangeInterval = 2f; // Time in seconds between direction changes
    public float jumpForce = 300f;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    private Animator anim;
    private Rigidbody rb;

    private Vector3 currentDirection = Vector3.zero;
    private float directionTimer = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Pick an initial random direction
        ChooseNewRandomDirection();
    }

    void Update()
    {
        Wander();
    }

    void Wander()
    {
        directionTimer += Time.deltaTime;

        // After 'directionChangeInterval' seconds, pick a new direction
        if (directionTimer >= directionChangeInterval)
        {
            directionTimer = 0f;
            ChooseNewRandomDirection();
        }

        // Move if we have a direction
        if (currentDirection != Vector3.zero)
        {
            // Rotate smoothly towards the chosen direction
            Quaternion lookRotation = Quaternion.LookRotation(currentDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.15f);

            // Move forward in the chosen direction
            transform.Translate(currentDirection * movementSpeed * Time.deltaTime, Space.World);

            // Set walking animation
            anim.SetInteger("Walk", 1);
        }
        else
        {
            // No movement direction, character stands still
            anim.SetInteger("Walk", 0);
        }

        // Optional: Random jump (uncomment if desired)
        /*
        if (Time.time > canJump && Random.value < 0.001f) // very small chance to jump occasionally
        {
            rb.AddForce(Vector3.up * jumpForce);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
        }
        */
    }

    void ChooseNewRandomDirection()
    {
        // Pick a random direction on the XZ plane
        float angle = Random.Range(0f, 360f);
        Vector3 newDir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));

        // Decide randomly whether to move or stand still
        // For example, 80% chance to move and 20% chance to remain idle
        if (Random.value > 0.2f)
        {
            currentDirection = newDir.normalized;
        }
        else
        {
            currentDirection = Vector3.zero;
        }
    }
}
