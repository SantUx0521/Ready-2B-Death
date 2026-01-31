using System;
using UnityEngine;

public class GlockSystem : MonoBehaviour
{
    // i'll use this one to manage the stats of every weapon in game, this one is for glock
    WeaponController weaponControl;
    public float Glock_range = 100f;
    public float Glock_Damage = 10; 
    public float spread = 0.05f;
    public int maxBullets = 15;
    public string ReloadName = "Glock_Reload";
    public string wepname = "Glock";
    public AudioSource shootSound;
    public AudioClip reloadSound;
    public Vector3 desviacion = new Vector3(1f, 2f, 3f);
    public float weaponForce = 40;
    public float cadence = 0.5f;
    void Start()
    {
        weaponControl = GetComponent<WeaponController>();
        weaponControl.range = Glock_range;
        weaponControl.damage = Glock_Damage;
        weaponControl.spread = spread;
        weaponControl.maxBullets = maxBullets;
        weaponControl.bullets = maxBullets;
        weaponControl.ReloadName = ReloadName;
        weaponControl.weaponForce = weaponForce;
        weaponControl.cadence = cadence;
        weaponControl.shootSound = shootSound;
        weaponControl.reloadSound = reloadSound;
        weaponControl.desviacion = desviacion;
    }
}
