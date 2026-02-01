using UnityEngine;

public class CloseAnyDoor : MonoBehaviour
{
    public DoorFunction door;
    private bool hasActivated = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            door.ForceLock();
            hasActivated = true;
        }   
    }
}
