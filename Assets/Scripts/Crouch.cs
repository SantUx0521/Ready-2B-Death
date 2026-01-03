using UnityEngine;

public class Crouch : MonoBehaviour
{
    public float crouchHeight;

    public void Crouching()
    {
        Vector3 originalPos = transform.localPosition;
        transform.localPosition = new Vector3(originalPos.x, crouchHeight, originalPos.z);
    }

    public void Stop()
    {
        
    }
}
