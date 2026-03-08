using UnityEngine;

public class AdvancedWeaponInertia : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Rotation")]
    public float rotationLag = 10f;
    public float yawAmount = 4f;
    public float pitchAmount = 2f;

    [Header("Position")]
    public float positionLag = 12f;
    public float positionAmount = 0.02f;

    [Header("Clamp")]
    public float maxYaw = 8f;
    public float maxPitch = 5f;
    public float maxPosition = 0.03f;

    private Quaternion lastCameraRot;

    private Vector3 rotationOffset;
    private Vector3 positionOffset;

    private Vector3 initialPos;
    private Quaternion initialRot;

    void Start()
    {
        lastCameraRot = cameraTransform.rotation;

        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }

    void LateUpdate()
    {
        Quaternion delta = cameraTransform.rotation * Quaternion.Inverse(lastCameraRot);
        Vector3 deltaEuler = NormalizeAngles(delta.eulerAngles);

        float yaw = Mathf.Clamp(-deltaEuler.y * yawAmount, -maxYaw, maxYaw);
        float pitch = Mathf.Clamp(deltaEuler.x * pitchAmount, -maxPitch, maxPitch);

        Vector3 targetRot = new Vector3(pitch, yaw, -yaw * 0.5f);

        rotationOffset = Vector3.Lerp(rotationOffset, targetRot, Time.deltaTime * rotationLag);

        Vector3 camDelta = cameraTransform.position - lastCameraRot * Vector3.zero;
        Vector3 targetPos = Vector3.ClampMagnitude(-camDelta * positionAmount * 100f, maxPosition);

        positionOffset = Vector3.Lerp(positionOffset, targetPos, Time.deltaTime * positionLag);

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            Quaternion.Euler(rotationOffset) * initialRot,
            Time.deltaTime * rotationLag
        );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialPos + positionOffset,
            Time.deltaTime * positionLag
        );

        lastCameraRot = cameraTransform.rotation;
    }

    Vector3 NormalizeAngles(Vector3 angles)
    {
        angles.x = Normalize(angles.x);
        angles.y = Normalize(angles.y);
        angles.z = Normalize(angles.z);
        return angles;
    }

    float Normalize(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}