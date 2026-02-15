using UnityEngine;

public class SpawnOnLevel : MonoBehaviour
{
    void Start()
    {
        if(PlayerController.Instance != null)
        {
            PlayerController.Instance.transform.position = transform.position;
        }
    }
}
