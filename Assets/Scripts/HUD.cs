using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject[] Icons;
    public PlayerWeaponController playerWeapon;
    public WeaponController weapon;
    private Fade fade;
    public Image granadeIcon;
    public float duration;
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

}
