using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContoller : MonoBehaviour
{
    private Animator animation_controller;
    private CharacterController character_controller;
    public Vector3 movement_direction;
    public float walking_velocity;
    public float velocity;
    public int num_lives;
    private float gravity = -9.81f;
    private float verticalVelocity = 0.0f;

    public bool canMove = true;
    private bool isDead = false;

    public LiveManager liveManager; // Reference to LiveManager script

    void Start()
    {
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        movement_direction = Vector3.zero;
        walking_velocity = 1.5f;
        velocity = 0.0f;

        // Find the LiveManager in the scene
        liveManager = FindObjectOfType<LiveManager>();

        if (liveManager == null)
        {
            Debug.LogError("LiveManager not found in the scene!");
        }
    }

    void Update()
    {
        if (num_lives == 0)
        {
            TriggerDeath();
            return;
        }

        HandleMovementAndAnimation();
    }

    void HandleMovementAndAnimation()
    {
        if (!canMove)
            return;

        if (character_controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Rotation controls
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -90 * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0);
        }

        // Forward movement
        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftShift))
        {
            velocity = Mathf.Lerp(velocity, walking_velocity, Time.deltaTime * 2);
            animation_controller.SetBool("isWalking", true);
            animation_controller.SetBool("isRunning", false);
            animation_controller.SetBool("isIdle", false);
            movement_direction = transform.forward;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            velocity = Mathf.Lerp(velocity, walking_velocity * 2.0f, Time.deltaTime * 2);
            animation_controller.SetBool("isRunning", true);
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isIdle", false);
            movement_direction = transform.forward;
        }
        // Backward movement
        //

        else if (Input.GetKey(KeyCode.DownArrow))
        {

            velocity = Mathf.Lerp(velocity, walking_velocity, Time.deltaTime * 2);


            animation_controller.SetBool("isWalking", true);


            animation_controller.SetBool("isRunning", false);


            animation_controller.SetBool("isIdle", false);


            movement_direction = -transform.forward;
        }
        else
        {
            // Idle state
            velocity = Mathf.Lerp(velocity, 0, Time.deltaTime * 2);
            animation_controller.SetBool("isIdle", true);
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isRunning", false);
            movement_direction = Vector3.zero;
        }

        movement_direction.y = verticalVelocity;
        character_controller.Move(movement_direction * velocity * Time.deltaTime);
    }

    void TriggerDeath()
    {
        if (isDead)
            return;

        isDead = true;
        canMove = false;
        velocity = 0.0f;
        animation_controller.SetBool("isDead", true);
    }

    // **Collision detection**
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // if (hit.gameObject.CompareTag("Obstacle")) // Detect collision with objects tagged as 'Obstacle'
        // {
        //     Debug.Log("Collided with an obstacle!");

        //     if (liveManager != null)
        //     {
        //         liveManager.ReduceLives(); // Reduce lives
        //     }
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter!");
        if (other.gameObject.CompareTag("Obstacle")) // Detect collision with objects tagged as 'Obstacle'
        {
            Debug.Log("Collided with an obstacle!");

            if (liveManager != null)
            {
                liveManager.ReduceLives(); // Reduce lives
            }
        }
    }
}
