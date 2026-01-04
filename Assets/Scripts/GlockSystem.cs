using System;
using UnityEngine;

public class GlockSystem : MonoBehaviour
{
    // i'll use this one to manage the stats of every weapon in game, this one is for glock
    WeaponController weaponControl;
    public float Glock_range = 100f;
    public int Glock_Damage = 10; 
    public float spread = 0.05f;
    public int maxBullets = 9;
    void Start()
    {
        weaponControl = GetComponent<WeaponController>();
        weaponControl.range = Glock_range;
        weaponControl.damage = Glock_Damage;
        weaponControl.spread = spread;
        weaponControl.maxBullets = maxBullets;
    }
}
