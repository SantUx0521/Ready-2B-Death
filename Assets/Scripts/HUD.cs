using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject[] Icons;
    public PlayerWeaponController playerWeapon;
    private Fade fade;
    public Image granadeIcon;
    public float duration;
    public int activeIcon {get; private set;}
    void Start()
    {
        activeIcon = -1;
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
        if (playerWeapon.granadeAction.WasPressedThisFrame())
        {
            fade.FadeOut(granadeIcon, 2f);
        }
    }

}
