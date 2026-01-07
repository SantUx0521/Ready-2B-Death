using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Using as a universal base for weapons, so i can manage damage, recoil, range, etc from their own scripts
    [Header("Layers")]
    public LayerMask Hittable;
    public LayerMask Enemy;
    public LayerMask Head;
    [SerializeField] GameObject flash;
    [SerializeField] GameObject fire;
    [HideInInspector] public float range;
    [HideInInspector] public int damage;
    [HideInInspector] public float spread;
    [HideInInspector] public int bullets;
    [HideInInspector] public int maxBullets;
    [HideInInspector] public string ReloadName;

    CameraShake cameraShake;

    public bool canShoot = true;

    public bool reloading = false;
    private PlayerController playerController;
    private PlayerWeaponController playerWeapon;
    public Animator anim;
    private Transform cameraPlayer;
    void Start()
    {
        cameraPlayer = GameObject.FindWithTag("MainCamera").transform;
        playerController = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
        cameraShake = cameraPlayer.GetComponent<CameraShake>();
        playerWeapon = GetComponentInParent<PlayerWeaponController>();
    }

    void Update()
    {
        if(reloading){return;}
        Shoot();
        Debug.DrawRay(cameraPlayer.position, cameraPlayer.forward * range, Color.red);
        Reload();
    }

    private void Shoot()
    {
        if ((playerController.playerInput.actions["Fire"].triggered || playerController.playerInput.actions["Fire"].IsPressed())  && canShoot && bullets > 0)
        {
            anim.SetTrigger("Shoot");
            StartCoroutine(Flashlight());
            bullets -= 1;
            StartCoroutine(cameraShake.Shake(0.07f, 0.07f));
            BulletHit();
            StartCoroutine(Delay());
        }
        else if ((playerController.playerInput.actions["Fire"].triggered || playerController.playerInput.actions["Fire"].IsPressed()) && canShoot && bullets <= 0)
        {
            anim.SetTrigger("NoBullets");
        }
    }

    private void BulletHit()
    {
        LayerMask combinedMask = Enemy | Hittable | Head;
        Vector3 direction = cameraPlayer.forward;

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.position, (direction + Random.insideUnitSphere * spread).normalized, out hit, range, combinedMask))
        {
            if (((1 << hit.collider.gameObject.layer) & Enemy) != 0)
            {
                hit.collider.gameObject.GetComponent<Enemy>().takeDamage(damage);
            }
            else if(((1 << hit.collider.gameObject.layer) & Head) != 0)
            {
                hit.collider.gameObject.GetComponentInParent<Enemy>().headShot();
                Debug.Log("HEADSHOT MADAFAKA");
            }

            if (((1 << hit.collider.gameObject.layer) & Hittable) != 0)
            {
                return;
            }
        }
    }

    private IEnumerator Flashlight()
    {
        flash.SetActive(true);
        fire.SetActive(true);
        Vector3 currentEuler = fire.transform.localEulerAngles;
        currentEuler.y = Random.Range(-180, 180);
        fire.transform.localEulerAngles = currentEuler;
        yield return new WaitForSeconds(0.1f);
        flash.SetActive(false);
        fire.SetActive(false);
    }

    private void Reload()
    {
        if (playerController.playerInput.actions["Reload"].triggered && bullets < maxBullets && !reloading)
        {
            reloading = true;
            canShoot = false;
            playerWeapon.anim.SetTrigger(ReloadName);
        }
    }

    public void FinishReload()
    {
        bullets = maxBullets;
        canShoot = true;
        reloading = false;
    }

    private IEnumerator Delay()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }
}
