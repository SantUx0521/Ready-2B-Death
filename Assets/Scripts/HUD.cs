using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class HUD : MonoBehaviour
{
    public GameObject[] Icons;
    public GameObject normalPoint;
    public GameObject point;
    public GameObject shotgunPoint;
    public PlayerWeaponController playerWeapon;
    public WeaponController weapon;
    private Fade fade;
    public Image granadeIcon;
    public float duration;
    public TextMeshProUGUI alert;
    public int activeIcon {get; private set;}
    void Start()
    {
        activeIcon = 0;
        playerWeapon = GetComponent<PlayerWeaponController>();
        fade = GetComponent<Fade>();
    }
    void Update()
    {
        GranadeIcon();
    }

    public IEnumerator Pointer(bool isShotgun)
    {
        yield return null;
        if (isShotgun == true)
        {
            shotgunPoint.SetActive(true);
            normalPoint.SetActive(false);
        }
        else
        {
            normalPoint.SetActive(true);
            shotgunPoint.SetActive(false);
            point.SetActive(false);
        }
    }

    public void TakeOffPointer()
    {
        shotgunPoint.SetActive(false);
        normalPoint.SetActive(false);
    }

    public void ChangeIcon(int index)
    {
        if(activeIcon >= 0 && Icons[activeIcon] != null)
        {
            Icons[activeIcon].SetActive(false);
        }
        activeIcon = index;
        Icons[activeIcon].SetActive(true);
    }

    public void GranadeIcon()
    {
        if (playerWeapon.granadeAction.WasPressedThisFrame() && playerWeapon.actualGranades > 0)
        {
            fade.FadeOut(granadeIcon, 1f, 10f);
        }
        else if(playerWeapon.granadeAction.WasPressedThisFrame() && playerWeapon.actualGranades <= 0)
        {
            ColorUtility.TryParseHtmlString("#F45B69", out Color warningColor);
            granadeIcon.color = warningColor;
            fade.FadeOut(granadeIcon, 1f, 1f);
        }
    }

    public IEnumerator ShowAlert(LocalizedString textToPut)
    {
        alert.gameObject.SetActive(true);
        alert.text = textToPut.GetLocalizedString();
        yield return new WaitForSeconds(5f);
        alert.gameObject.SetActive(false);
    }

}
