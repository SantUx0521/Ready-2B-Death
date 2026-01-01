using System;
using UnityEngine;

public class GlockSystem : MonoBehaviour
{
    // i'll use this one to manage the stats of every weapon in game, this one is for glock
    WeaponController weaponControl;
    public float Glock_range = 100f;
    public float Glock_Damage = 10f; 
    void Start()
    {
        weaponControl = GetComponent<WeaponController>();
        weaponControl.range = Glock_range;
        weaponControl.damage = Glock_Damage;
    }
}
