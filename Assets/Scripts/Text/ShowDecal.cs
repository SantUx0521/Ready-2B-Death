using UnityEngine;

public class ShowDecal : MonoBehaviour
{
    [SerializeField] GameObject Decal;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Decal.SetActive(true);
            Destroy(gameObject);
        }
    }
}
