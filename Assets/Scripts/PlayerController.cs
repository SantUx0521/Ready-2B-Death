using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // Used for managing player's movement XD
    [Header("Camera")]
    public Transform playerCamera; 
    public float mouseSensitivityX = 30f;
    public float mouseSensitivityY = 40f;
    private float angleY;

    [Header("Movement")]
    public float velocity = 1f;
    public PlayerInput playerInput;
    private InputAction moveAction;
    private Vector3 playerVelocity;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Jumping")]

    private InputAction jumpAction;
    private readonly float JumpHeight = 1f;
    public float gravity = -9.8f;
    private bool isGrounded;
    private CharacterController characterController;

    [Header("Crouch")]
    private InputAction crouchAction;
    public float crouchHeight;
    public bool isCrouching = false;
    private float actualHeight;
    private float actualVelocity;

    [Header("Sprint")]
    private InputAction sprintAction;
    public float sprintVelocity = 15f;
    private bool isSprinting = false;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        crouchAction = playerInput.actions["Crouch"];
        sprintAction = playerInput.actions["Sprint"];
        actualHeight = characterController.height;
        actualVelocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraMovement();
        Crouch();
        Run();
    }

    private void Move()
    {

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveInput = new(input.x, 0f, input.y);
        moveInput = transform.TransformDirection(moveInput) * velocity;
        characterController.Move(moveInput * Time.deltaTime);

        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        else
        {
            playerVelocity.y += gravity * Time.deltaTime; 
        }
        characterController.Move(playerVelocity * Time.deltaTime);

        // Handle Jumping
        if (jumpAction.triggered && characterController.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);        
    }

    private void CameraMovement()
    {
        // Look X
        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        moveDirection.x = lookInput.x * mouseSensitivityX * Time.deltaTime;
        moveDirection.y = lookInput.y * mouseSensitivityY * Time.deltaTime;

        transform.Rotate(Vector3.up * moveDirection.x); 

        // Look Y
        angleY -= moveDirection.y;
        angleY = Mathf.Clamp(angleY, -70f, 70f);
        playerCamera.localRotation = Quaternion.Euler(angleY, 0, 0);

    }

    private void Crouch()
    {
        if (crouchAction.WasPressedThisFrame() && !isCrouching)
        {
            characterController.height = crouchHeight;
            velocity /= 1.5f;
            isCrouching = true;
        }
        else if (crouchAction.WasPressedThisFrame() && isCrouching)
        {
            characterController.height = actualHeight;
            velocity = actualVelocity;
            isCrouching = false;
        }
    }

    private void Run()
    {
        if((sprintAction.IsPressed() || sprintAction.WasPressedThisFrame()) && moveAction.IsPressed())
        {
            isSprinting = true;
            isCrouching = false;
            characterController.height = actualHeight;
            velocity = sprintVelocity;
        }
        else if (isSprinting && moveAction.WasReleasedThisFrame()){
            isSprinting = false;
            velocity = actualVelocity;
        }
    }
}
