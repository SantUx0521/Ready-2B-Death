using UnityEngine;

public class Figure8Movement : MonoBehaviour
{
    private Vector3 previousMovement;

    public float speed = 1;
    public float xScale = 1;
    public float yScale = 1;
    void Start()
    {
        previousMovement = Vector3.zero;
    }

    void LateUpdate()
    {
        Vector3 currentMovement = Vector3.right * Mathf.Sin(Time.timeSinceLevelLoad / 2 * speed) * xScale - Vector3.up * Mathf.Sin(Time.timeSinceLevelLoad * speed) * yScale;
        transform.localPosition += currentMovement - previousMovement;
        previousMovement = currentMovement;
    }
}
