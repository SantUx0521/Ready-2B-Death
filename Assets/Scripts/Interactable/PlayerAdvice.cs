using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlayerAdvice : MonoBehaviour
{
    GameObject player;
    public LocalizedString textToPut;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void ShowAdvice()
    {
        StartCoroutine(player.GetComponent<HUD>().ShowAlert(textToPut));
    }
}
