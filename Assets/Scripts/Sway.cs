using UnityEngine;
using UnityEngine.InputSystem;

public class Swing : MonoBehaviour
{
    // Specific script to make the sway of every weapon when moving the camera, i have it only for glock for now
    // Hay comentarios en ingles porque ns, a veces me gusta ser yo mismo
    [Header("Trans XD")]
    public float amount;
    public PlayerInput playerInput;
    public PlayerWeaponController playerWeapon;
    public float maxSway;
    public float smoothness;
    
    [Header("Rotational")]
    public float tiltAmount;
    public float maxTilt;
    public float tiltSmoothness;

    public bool tiltDirX, tiltDirY, tiltDirZ;

    Vector3 initialPosition;
    Quaternion initialRotation;
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerWeapon = GetComponentInParent<PlayerWeaponController>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if(playerWeapon.isAiming)
        {
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
        return;
        }
        Sway();
        rotationalSway();
    }

    void Sway()
    {
        Vector2 lookInput = playerInput.actions["Look"].ReadValue<Vector2>();
        float clampedX = Mathf.Clamp(lookInput.x * amount, -maxSway, maxSway);
        float clampedY = Mathf.Clamp(lookInput.y * amount, -maxSway, maxSway);
        Vector3 finalPos = new Vector3(clampedX, 0, clampedY);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + finalPos, Time.deltaTime * smoothness);
    }

    void rotationalSway()
    {
        Vector2 tiltInput = playerInput.actions["Look"].ReadValue<Vector2>();
        tiltInput.x = Mathf.Clamp(tiltInput.x * tiltAmount, -maxTilt, maxTilt);
        tiltInput.y = Mathf.Clamp(tiltInput.y * tiltAmount, -maxTilt, maxTilt);
        Quaternion finalRot = Quaternion.Euler(new Vector3(tiltDirX ? -tiltInput.y : 0, tiltDirY ? tiltInput.x : 0, tiltDirZ ? tiltInput.x : 0));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, initialRotation * finalRot, Time.deltaTime * tiltSmoothness);
    }
}
