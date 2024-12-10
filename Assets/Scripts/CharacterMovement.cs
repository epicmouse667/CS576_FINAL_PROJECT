using UnityEngine;

namespace YourNamespace
{
    public class CharacterMovement : MonoBehaviour
    {
        private Animator animation_controller;
        private CharacterController character_controller;
        public Vector3 movement_direction;
        public float walking_velocity;
        public float velocity;
        public int num_lives;
        public bool canMove = true;

        // Jump and Gravity Variables
        private bool isJumping = false;
        private float jumpHeight = 4.0f;
        private float gravity = -9.81f;
        private float verticalVelocity = 0.0f;
        private bool isGrounded = true;
        private bool isDead = false;

        void Start()
        {
            animation_controller = GetComponent<Animator>();
            character_controller = GetComponent<CharacterController>();
            movement_direction = Vector3.zero;
            walking_velocity = 5.0f;
            velocity = 8.0f;
            num_lives = 5;
        }

        void Update()
        {
            if (num_lives == 0)
            {
                TriggerDeath();
                return;
            }

            ResetAnimationStates();

            isGrounded = CheckGrounded();
            if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -1f; // Prevent sliding down when grounded
            }

            HandleMovementAndAnimation();
            HandleRotation();
            MoveCharacter();
            AdjustCollider();
        }

        void ResetAnimationStates()
        {
            animation_controller.SetBool("isWalkForwards", false);
            animation_controller.SetBool("isWalkBackwards", false);
            animation_controller.SetBool("isCrouchForwards", false);
            animation_controller.SetBool("isCrouchBackwards", false);
            animation_controller.SetBool("isIdle", false);
            animation_controller.SetBool("isRunning", false);
        }

        void HandleMovementAndAnimation()
        {
            if (!canMove) return;

            if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
            {
                velocity = walking_velocity;
                animation_controller.SetBool("isWalkForwards", true);
                movement_direction = transform.forward;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftControl))
            {
                velocity = walking_velocity / 1.5f;
                animation_controller.SetBool("isWalkBackwards", true);
                movement_direction = -transform.forward;
            }
            else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftControl))
            {
                velocity = walking_velocity / 2.0f;
                animation_controller.SetBool("isCrouchForwards", true);
                movement_direction = transform.forward;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftControl))
            {
                velocity = walking_velocity / 2.0f;
                animation_controller.SetBool("isCrouchBackwards", true);
                movement_direction = -transform.forward;
            }
            else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift))
            {
                velocity = walking_velocity * 2.0f;
                animation_controller.SetBool("isRunning", true);
                movement_direction = transform.forward;
            }
            else
            {
                velocity = 0.0f; // Stop character movement
                animation_controller.SetBool("isIdle", true);
                movement_direction = Vector3.zero;
            }
        }

        void HandleRotation()
        {
            if (!isJumping && !animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Rotate(0, -90 * Time.deltaTime, 0);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.Rotate(0, 90 * Time.deltaTime, 0);
                }
            }
        }

        void MoveCharacter()
        {
            movement_direction.Normalize();

            if (isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animation_controller.SetTrigger("Jump");
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    isJumping = true;
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime; // Apply gravity only when not grounded
            }

            movement_direction.y = verticalVelocity;
            character_controller.Move(movement_direction * velocity * Time.deltaTime);
        }

        bool CheckGrounded()
        {
            float rayLength = 0.2f;
            RaycastHit hit;
            Vector3 rayStart = transform.position + Vector3.up * 0.1f;

            Debug.DrawRay(rayStart, Vector3.down * rayLength, Color.green);
            return Physics.Raycast(rayStart, Vector3.down, out hit, rayLength);
        }

        void AdjustCollider()
        {
            bool isCrouching = animation_controller.GetCurrentAnimatorStateInfo(0).IsName("CrouchForwards") ||
                               animation_controller.GetCurrentAnimatorStateInfo(0).IsName("CrouchBackwards");

            if (isCrouching)
            {
                character_controller.height = 1.0f;
                character_controller.center = new Vector3(0, 0.5f, 0);
            }
            else
            {
                character_controller.height = 2.0f;
                character_controller.center = new Vector3(0, 1.0f, 0);
            }
        }

        void TriggerDeath()
        {
            if (isDead) return;

            isDead = true;
            animation_controller.SetBool("isDead", true);
            velocity = 0.0f;
            canMove = false;
        }
    }
}
