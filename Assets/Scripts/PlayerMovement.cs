using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 10f;
    public float acceleration = 10f;
    public float airControlMultiplier = 0.6f;

    [Header("Jumping")]
    public float jumpHeight = 2.5f;
    public int maxJumps = 2;

    [Header("Gravity")]
    public float gravity = -25f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrainPerSecond = 30f;
    public float staminaRegenPerSecond = 20f;
    [Range(0f, 1f)] public float sprintRecoveryThreshold = 0.25f; 

    [Header("Sprint")]
    public float sprintSpeed = 18f;

    [Header("Camera")]
    public Camera playerCamera;
    public float normalFOV = 75f;
    public float sprintFOV = 90f;
    public float fovLerpSpeed = 8f;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentMoveVelocity;

    private bool isGrounded;
    private int jumpsRemaining;
    private bool isSprinting;

    private float currentStamina;
    private bool canSprint = true; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        jumpsRemaining = maxJumps;
        currentStamina = maxStamina;

        if (playerCamera != null)
            playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        HandleGravity();
        HandleStamina();
        HandleCameraFOV();
    }

    void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

            jumpsRemaining = maxJumps;
        }
    }

    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = (transform.right * x + transform.forward * z).normalized;

        isSprinting = Input.GetKey(KeyCode.LeftShift)
                      && canSprint
                      && z > 0f
                      && inputDirection.magnitude > 0.1f;

        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 targetVelocity = inputDirection * targetSpeed;

        float control = isGrounded ? 1f : airControlMultiplier;

        currentMoveVelocity = Vector3.Lerp(
            currentMoveVelocity,
            targetVelocity,
            acceleration * control * Time.deltaTime
        );

        controller.Move(currentMoveVelocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsRemaining--;
        }
    }

    void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleStamina()
    {
        if (isSprinting)
        {
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canSprint = false; 
            }
        }
        else
        {
            currentStamina += staminaRegenPerSecond * Time.deltaTime;

            
            if (currentStamina >= maxStamina * sprintRecoveryThreshold)
            {
                canSprint = true;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    void HandleCameraFOV()
    {
        if (playerCamera == null) return;

        float targetFOV = isSprinting ? sprintFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            fovLerpSpeed * Time.deltaTime
        );
    }

    // Para UI
    public float GetStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }
}
