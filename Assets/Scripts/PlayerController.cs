using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // Used for managing player's movement XD
    [Header("CameraP")]
    public Transform playerCamera; 
    public float mouseSensitivityX = 30f;
    public float mouseSensitivityY = 40f;
    private float angleY;
    public Volume volume;
    GameManager gameManager;

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
    public float actualVelocity;

    [Header("Sprint")]
    private InputAction sprintAction;
    public float sprintVelocity = 15f;
    public bool isSprinting = false;
    Camera cameraP;
    private float originalFOV;
    private float adsSpeed = 15f;
    PlayerWeaponController playerWeapon;

    [Header("Flashlight")]
    private InputAction flashlightAction;
    [SerializeField] private GameObject flashlight;
    private bool flashing = false;

    [Header("Interact")]
    public LayerMask InteractableLayer;
    public bool isInteractable = false;
    InputAction interactAction;
    public float interactRange;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerCamera = GetComponentInChildren<Camera>().transform;
        playerWeapon = GetComponent<PlayerWeaponController>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        crouchAction = playerInput.actions["Crouch"];
        sprintAction = playerInput.actions["Sprint"];
        flashlightAction = playerInput.actions["Flashlight"];
        interactAction = playerInput.actions["Interact"];
        gameManager = GetComponent<GameManager>();
        actualHeight = characterController.height;
        actualVelocity = velocity;
        cameraP = GetComponentInChildren<Camera>();
        originalFOV = cameraP.fieldOfView;
    }

    void Update()
    {
        Move();
        CameraMovement();
        Crouch();
        Run();
        Flashlight();
        Interactable();
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
        if ((jumpAction.triggered || jumpAction.IsPressed()) && characterController.isGrounded)
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
        angleY = Mathf.Clamp(angleY, -60f, 60f);
        playerCamera.localRotation = Quaternion.Euler(angleY, 0, 0);
    }

    private void Crouch()
    {
        if (crouchAction.WasPressedThisFrame() && !isCrouching)
        {
            characterController.height = crouchHeight;
            velocity = 6.5f;
            isCrouching = true;
            if (volume.profile.TryGet<UnityEngine.Rendering.Universal.Vignette>(out var vignette))
            {
                float crouchVignette = Mathf.Lerp(0.33f, 0.12f, 0.1f);
                vignette.intensity.Override(crouchVignette);
            }
        }
        else if (crouchAction.WasPressedThisFrame() && isCrouching)
        {
            characterController.height = actualHeight;
            velocity = actualVelocity;
            isCrouching = false;
            if (volume.profile.TryGet<UnityEngine.Rendering.Universal.Vignette>(out var vignette))
            {
                float crouchVignette = Mathf.Lerp(0.12f, 0.33f, 0.1f);
                vignette.intensity.Override(crouchVignette);
            }
            
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
            cameraP.fieldOfView = Mathf.Lerp(cameraP.fieldOfView, originalFOV + 20f, Time.deltaTime * adsSpeed);
        }
        else if (isSprinting && moveAction.WasReleasedThisFrame()){
            isSprinting = false;
            velocity = actualVelocity;
            cameraP.fieldOfView = Mathf.Lerp(cameraP.fieldOfView, originalFOV, Time.deltaTime * adsSpeed);
        }
        if (playerWeapon.isAiming)
        {
            isSprinting = false;
            velocity = actualVelocity;
        }
    }

    private void Flashlight()
    {
        if (flashlightAction.WasPressedThisFrame() && !flashing)
        {
            flashing = true;
            flashlight.SetActive(true);
        }
        else if (flashlightAction.WasPressedThisFrame() && flashing)
        {
            flashing = false;
            flashlight.SetActive(false);
        }
    }

    private void Interactable()
    {
        RaycastHit Interact;
        if (Physics.Raycast(cameraP.transform.position, cameraP.transform.forward, out Interact, interactRange, InteractableLayer))
        {
            if (((1 << Interact.collider.gameObject.layer) & InteractableLayer) != 0)
            {
                isInteractable = true;
                Interact.collider.gameObject.GetComponent<InteractableObject>().state = true;
                Interact.collider.gameObject.GetComponent<StaticText>().ShowText();
            }
        }
        else
        {
            isInteractable = false;
        }

        if (isInteractable && interactAction.WasPressedThisFrame())
        {
            gameManager.HUD.SetActive(false);
            gameManager.Pause();
            gameManager.Notes.SetActive(true);
            gameManager.isMenuOn = true;
        }
    }
}
