using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    // movement and gravity
    private Vector3 velocity;
    public float gravity = -9.81f;
    public float groundedGravity = -1f;

    // Ground check
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;
    private bool isGrounded;

    // player movement
    public float moveSpeed = 3.5f;
    public float sprintMultiplier = 1.8f;

    // dependecies
    public Animator animator;
    public Transform cameraTransform;

    private CharacterController controller;
    #endregion

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundedGravity; // Keep player in the ground
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Input movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0, vertical);
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 moveDir = Vector3.zero;

        if (input.magnitude >= 0.1f)
        {
            // Movement related to the camera
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;

            moveDir = camForward.normalized * input.z + camRight.normalized * input.x;

            // Rotate towards movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.2f);
        }

        // Final speed
        float currentSpeed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        // Animation based on horizontal movement
        float animSpeed = new Vector3(moveDir.x, 0, moveDir.z).magnitude * currentSpeed;
        animator.SetFloat("Speed", animSpeed);

        // Mixed movement
        Vector3 finalVelocity = (moveDir * currentSpeed) + velocity;
        controller.Move(finalVelocity * Time.deltaTime);
    }
}
