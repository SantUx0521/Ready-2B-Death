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

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        cameraMovement();
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

        if (jumpAction.triggered && characterController.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);        
    }

    private void cameraMovement()
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
}
