using UnityEngine;
using UnityEngine.Localization;

public class MedKit : MonoBehaviour
{
    public int HealingAmount;
    public LocalizedString textToPut;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager player = other.gameObject.GetComponent<GameManager>();
            if(player.currentHealth < player.MaxHealth)
            {
                player.Heal(HealingAmount);
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(other.gameObject.GetComponent<HUD>().ShowAlert(textToPut));
            }
        }
    }
}
