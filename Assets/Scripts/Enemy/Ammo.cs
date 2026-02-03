using UnityEngine;

public class Ammo : MonoBehaviour
{
    public string tipe; 
    public int amount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerWeaponController>().GetAmmo(tipe, amount);
            Destroy(gameObject);
        }
    }
}
