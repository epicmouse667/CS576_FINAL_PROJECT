using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioSource audioSource; // Audio source component
    public AudioClip gameOverSound;
    public int puzzleCollected = 0;

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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource missing. Adding one to the GameObject.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (num_lives == 0)
        {
            TriggerDeath();
            return;
        }
        if (puzzleCollected == 4)
        {
            SceneManager.LoadScene("Facts");
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
        bool isShiftpressed = Input.GetKey(KeyCode.LeftShift);
        if (isShiftpressed && Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Running");
            velocity = Mathf.Lerp(velocity, walking_velocity * 2.0f, Time.deltaTime * 2);
            animation_controller.SetBool("isRunning", true);
            animation_controller.SetBool("isWalking", false);
            animation_controller.SetBool("isIdle", false);
            movement_direction = transform.forward;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !isShiftpressed)
        {
            Debug.Log("walk");

            velocity = Mathf.Lerp(velocity, walking_velocity, Time.deltaTime * 2);
            animation_controller.SetBool("isWalking", true);
            animation_controller.SetBool("isRunning", false);
            animation_controller.SetBool("isIdle", false);
            movement_direction = transform.forward;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("back");

            velocity = Mathf.Lerp(velocity, walking_velocity, Time.deltaTime * 2);
            animation_controller.SetBool("isWalking", true);
            animation_controller.SetBool("isRunning", false);
            animation_controller.SetBool("isIdle", false);
            movement_direction = -transform.forward;
        }
        else
        {
            Debug.Log("stop");
            // Idle state
            velocity = 0;
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
         // Play game over sound if available
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
            Debug.Log("Game Over sound played.");
        }
        else
        {
            Debug.LogWarning("AudioSource or GameOverSound not assigned!");
        }
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
