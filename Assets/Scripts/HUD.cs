using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject[] Icons;
    public int activeIcon {get; private set;}
    void Start()
    {
        activeIcon = -1;
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
}
