using UnityEngine;

public class ChargeNewZone : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] LastZone;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(enemies != null)
            {
                foreach(GameObject enemy in enemies)
                {
                    enemy.SetActive(true);
                }
            }
        
            if(LastZone != null)
            {
                foreach(GameObject block in LastZone)
                {
                    Destroy(block);
                }
            }
            Destroy(gameObject);
        }
    }
}
