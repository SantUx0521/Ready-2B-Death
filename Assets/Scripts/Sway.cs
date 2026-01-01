using UnityEngine;
using UnityEngine.InputSystem;

public class Swing : MonoBehaviour
{
    public float amount;
    public PlayerInput playerInput;
    public float maxSway;
    public float smoothness;
    
    public float tiltAmount;
    public float maxTilt;
    public float tiltSmoothness;

    public bool tiltDirX, tiltDirY, tiltDirZ;

    Vector3 initialPosition;
    Quaternion initialRotation;
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        
    }

    void Update()
    {
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
