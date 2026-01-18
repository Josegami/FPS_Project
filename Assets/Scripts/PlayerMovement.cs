using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float acceleration = 12f;
    public float berserkSpeedMultiplier = 1.5f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 2.2f;
    public float gravity = -30f;

    [Header("Mouse Look")]
    public Transform cameraPivot;
    public float mouseSensitivity = 2.5f;
    public float maxLookUp = 80f;
    public float maxLookDown = -80f;

    [Header("Detection")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrainPerSecond = 25f;
    public float staminaRegenPerSecond = 20f;
    [Range(0f, 1f)] public float sprintRecoveryThreshold = 0.25f;

    [Header("FOV Settings")]
    public Camera playerCamera;
    public float normalFOV = 60f;   
    public float sprintFOV = 75f;    
    public float fovSpeed = 10f;    

    private CharacterController controller;
    private Vector3 verticalVelocity;
    private Vector3 currentMoveVelocity;

    private float xRotation;
    private bool isGrounded;
    private bool isSprinting;

    private float currentStamina;
    private bool canSprint = true;

    [SerializeField] private Animator animator;

    private Player player;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();

        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleGroundCheck();
        HandleMouseLook();
        HandleMovementAndGravity();
        HandleStamina();
        HandleFOV();
    }

    void HandleGroundCheck()
    {
        if (groundCheck != null)
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        else
            isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity.y < 0)
            verticalVelocity.y = -2f;
    }

    void HandleMouseLook()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.isPaused)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookDown, maxLookUp);
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovementAndGravity()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = (transform.right * x + transform.forward * z).normalized;

        isSprinting = Input.GetKey(KeyCode.LeftShift) && canSprint && z > 0.1f && inputDir.magnitude > 0.1f;

        float currentWalkSpeed = player.isBerserkActive ? walkSpeed * berserkSpeedMultiplier : walkSpeed;
        float currentSprintSpeed = player.isBerserkActive ? sprintSpeed * berserkSpeedMultiplier : sprintSpeed;

        float targetSpeed = isSprinting ? currentSprintSpeed : currentWalkSpeed;
        Vector3 targetVelocity = inputDir * targetSpeed;

        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetVelocity, acceleration * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;

        controller.Move((currentMoveVelocity + verticalVelocity) * Time.deltaTime);

        if (animator != null && animator.hasBoundPlayables)
        {
            Vector3 localVel = transform.InverseTransformDirection(currentMoveVelocity);
            animator.SetFloat("VelocityX", localVel.x);
            animator.SetFloat("VelocityZ", localVel.z);
        }
    }

    void HandleFOV()
    {
        if (playerCamera == null) return;

        bool isMoving = controller.velocity.magnitude > 0.1f;
        float targetFOV = (isSprinting && isMoving) ? sprintFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);
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
                canSprint = true;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    public float GetStaminaNormalized() => currentStamina / maxStamina;
}