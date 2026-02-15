using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameManager instance;
    public UnityEngine.UI.Image HealthBar;
    public GameObject HealthContainer;
    public GameObject of;
    public TextMeshProUGUI HealthCounter;
    public GameObject HUD;
    private bool noUI;
    public GameObject Notes;
    private GameObject activeCanvas;
    public PauseMenu pauseMenu;
    public float currentHealth;
    private Color originalColor;
    private Color originalTextColor;
    private PlayerController player;
    private PlayerWeaponController playerWeap;
    public List<string> playerKeys = new List<string>();
    [HideInInspector] public float MaxHealth = 100;
    public bool isMenuOn;
    public bool isDead;
    public GameObject DeathScreen;

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
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
        HideUI();
        UpdateBar();
        Die();
        InMenu();
    }

    private void HideUI()
    {
        if (player.playerInput.actions["HideHud"].WasPressedThisFrame() && !noUI)
        {
            HUD.SetActive(false);
            noUI = true;
        }
        else if(player.playerInput.actions["HideHud"].WasPressedThisFrame() && noUI)
        {
            HUD.SetActive(true);
            noUI = false;
        }
    }

    private void ShowUI()
    {
        if(playerWeap.activeWeaponIndex >= 0)
        {
            HealthBar.gameObject.SetActive(true);
            HealthContainer.SetActive(true);
            HealthCounter.gameObject.SetActive(true);
            of.SetActive(true);
        }
    }

    public void Heal(int healAmount)
    {
        if(currentHealth < MaxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, MaxHealth);
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
            isDead = true;
            DeathScreen.SetActive(true);
            Pause();
            player.playerInput.SwitchCurrentActionMap("UI");
            Cursor.visible = true;
        } 
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        HUD.SetActive(true);
        Notes.SetActive(false);
        pauseMenu.bG.SetActive(false);
        pauseMenu.pauseMenu.SetActive(false);
        if(activeCanvas != null)
        {
            activeCanvas.SetActive(false);
        }
        isMenuOn = false;
        player.playerInput.SwitchCurrentActionMap("Player");
        AudioSource[] allAudios = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        for(int i = 0; i < allAudios.Length; i++)
        {
            allAudios[i].Play();
        }
    }
    

    public void GetActiveCanva(GameObject canva)
    {
        activeCanvas = canva;
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
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit(); //Luego le coloco un aviso de si realmente quiere cerrar el juego por si aca xd
    }

}
