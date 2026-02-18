using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class OpenElevatorInside : MonoBehaviour
{
    public GameObject elevator;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OpenElevator();
        }
    }

    void OpenElevator()
    {
        elevator.GetComponent<DoorFunction>().Interact();
        Destroy(gameObject);
    }
}
