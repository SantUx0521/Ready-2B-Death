using UnityEngine;

public class Pump_System : MonoBehaviour
{
    WeaponController weaponControl;
    public float pump_range = 50f;
    public float pump_Damage = 30; 
    public float spread = 0.6f;
    public int maxBullets = 8;
    public string ReloadName = "pump_Reload";
    public string wepname = "Pump";
    public float weaponForce = 100;
    public float cadence = 1.5f;
    public Vector3 desviacion = new Vector3(1f, 2f, 3f);
    public AudioSource shootSound;
    public AudioClip reloadSound;
    public bool isShotgun = true;
    
    void Start()
    {
        weaponControl = GetComponent<WeaponController>();
        weaponControl.range = pump_range;
        weaponControl.damage = pump_Damage;
        weaponControl.spread = spread;
        weaponControl.maxBullets = maxBullets;
        weaponControl.bullets = maxBullets;
        weaponControl.ReloadName = ReloadName;
        weaponControl.weaponForce = weaponForce;
        weaponControl.cadence = cadence;
        weaponControl.desviacion = desviacion;
        weaponControl.audioSource = shootSound;
        weaponControl.isShotgun = isShotgun;
        weaponControl.wepname = wepname;
    }
}
