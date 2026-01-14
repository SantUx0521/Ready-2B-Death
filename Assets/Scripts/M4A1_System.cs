using UnityEngine;

public class M4A1_System : MonoBehaviour
{
    WeaponController weaponControl;
    public float Glock_range = 200f;
    public int Glock_Damage = 10; 
    public float spread = 0.6f;
    public int maxBullets = 30;
    public string ReloadName = "M4A1_Reload";
    public float weaponForce = 60;
    public float cadence = 0.3f;
    public Vector3 desviacion = new Vector3(1f, 2f, 3f);
    
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
        weaponControl.desviacion = desviacion;
    }
}
