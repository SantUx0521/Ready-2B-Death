using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
    public GameObject[] enemiesToActivate;
    bool wasActivated = false;
    void OnTriggerEnter(Collider collision)
    {
        if (!wasActivated && collision.gameObject.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemiesToActivate)
            {
                enemy.SetActive(true);
            }
            wasActivated = true;
        }
    }
}
