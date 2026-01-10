using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public UnityEngine.UI.Image HealthBar;
    public TextMeshProUGUI HealthCounter;
    public float currentHealth;
    private Color originalColor;
    private Color originalTextColor;
    private PlayerController player;
    [HideInInspector] public float MaxHealth = 100;
    void Start()
    {
        currentHealth = MaxHealth;
        originalColor = HealthBar.color;
        originalTextColor = HealthCounter.color;
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        UpdateBar();
        Die();
    }

    public void Heal(int healAmount)
    {
        if(currentHealth < MaxHealth)
        {
            currentHealth += healAmount;
        }
        else
        {
            Debug.Log("Tu salud está al maximo");
        }
        
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth <= 0) return;
        currentHealth -= damage;
    }

    private void UpdateBar()
    {
        HealthBar.fillAmount = currentHealth / MaxHealth;
        HealthCounter.text = Mathf.RoundToInt(currentHealth).ToString();

        if(currentHealth < 40)
        {
            ColorUtility.TryParseHtmlString("#F45B69", out Color warningColor);
            HealthBar.color = warningColor;
            HealthCounter.color = Color.darkRed;
            player.volume.profile.TryGet<UnityEngine.Rendering.Universal.Vignette>(out var vignette);
            vignette.intensity.Override(0.35f);
            vignette.color.value = Color.darkRed;

        }
        else
        {
            HealthBar.color = originalColor;
            HealthCounter.color = originalTextColor;
            player.volume.profile.TryGet<UnityEngine.Rendering.Universal.Vignette>(out var vignette);
            vignette.intensity.Override(0.12f);
            vignette.color.value = Color.black;
        }
    }
    private void Die()
    {
        if(currentHealth <= 0)
        {
            Debug.Log("Muelto brodel");
        } 
    }
}
