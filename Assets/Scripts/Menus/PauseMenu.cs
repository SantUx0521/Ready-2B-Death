using System;
using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    public GameObject pauseMenu;
    public GameObject bG;
    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();
        playerController = GetComponentInParent<PlayerController>();
    }
    void Update()
    {
        if (playerController.pauseAction.WasPressedThisFrame() && !gameManager.isMenuOn)
        {
            gameManager.isMenuOn = true;
            bG.SetActive(true);
            StartCoroutine(ShowPause());
        }
    }

    public IEnumerator ShowPause()
    {
        gameManager.Pause();
        yield return null;
        pauseMenu.SetActive(true);
        AudioSource[] allAudios = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        for(int i = 0; i < allAudios.Length; i++)
        {
            allAudios[i].Pause();
        }
    }
}
