using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public UnityEngine.UI.Image HealthBar;
    public GameObject of;
    public TextMeshProUGUI HealthCounter;
    public GameObject HUD;
    public GameObject Notes;
    public float currentHealth;
    private Color originalColor;
    private Color originalTextColor;
    private PlayerController player;
    private PlayerWeaponController playerWeap;
    public List<string> playerKeys = new List<string>();
    [HideInInspector] public float MaxHealth = 100;
    public bool isMenuOn;
    void Start()
    {
        Cursor.visible = false;
        currentHealth = MaxHealth;
        originalColor = HealthBar.color;
        originalTextColor = HealthCounter.color;
        player = GetComponent<PlayerController>();
        playerWeap = GetComponent<PlayerWeaponController>();
    }

    void Update()
    {
        ShowUI();
        UpdateBar();
        Die();
        InMenu();
    }

    private void ShowUI()
    {
        if(playerWeap.activeWeaponIndex >= 0)
        {
            HealthBar.gameObject.SetActive(true);
            of.SetActive(true);
        }
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
        } 
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void InMenu()
    {
        if (isMenuOn)
        {
            player.playerInput.SwitchCurrentActionMap("UI");
            Cursor.visible = true;
            if (player.playerInput.actions["Close"].WasPressedThisFrame())
            {
                Resume();
                Cursor.visible = false;
                HUD.SetActive(true);
                Notes.SetActive(false);
                isMenuOn = false;
                player.playerInput.SwitchCurrentActionMap("Player");
            }
        }
    }

}
