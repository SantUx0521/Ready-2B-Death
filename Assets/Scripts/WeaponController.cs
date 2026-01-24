using System.Collections;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using UnityEngine.Audio;
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
    [HideInInspector] public float damage;
    [HideInInspector] public float spread;
    [HideInInspector] public int bullets;
    [HideInInspector] public int maxBullets;
    [HideInInspector] public string ReloadName;
    [HideInInspector] public float cadence;
    [SerializeField] public int actualBulletsAmount;
    [HideInInspector] public float weaponForce;
    [HideInInspector] public Vector3 desviacion;
    [SerializeField] GameObject bulletHole;
    [SerializeField] GameObject bulletHoleContainer;
    [HideInInspector] public bool isShotgun;

    [SerializeField] AudioMixer shootAudioMixer;
    [HideInInspector] public AudioSource shootSound;

    CameraShake cameraShake;
    public TextMeshProUGUI bulletsText;
    public TextMeshProUGUI bulletsAmountText;
    public bool canShoot = true;

    public bool reloading = false;
    private PlayerController playerController;
    private PlayerWeaponController playerWeapon;
    public Animator anim;
    private Transform cameraPlayer;
    public static System.Action<Vector3, float> OnNoise;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        cameraPlayer = GameObject.FindWithTag("MainCamera").transform;
        playerController = GetComponentInParent<PlayerController>();
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
            if (isShotgun)
            {
                HandleShotgun();
            }
            else
            {
                BulletHit();
            }
            
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

        OnNoise?.Invoke(transform.position, 42f);
        shootSound.Stop();
        shootSound.Play();

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.position, (direction + UnityEngine.Random.insideUnitSphere * spread).normalized, out hit, range, combinedMask))
        {
            StartCoroutine(WhatItHits(hit, direction));
        }
    }

    private void HandleShotgun()
    {
        LayerMask combinedMask = Enemy | Hittable | Head;
        Vector3 direction = cameraPlayer.forward;
        OnNoise?.Invoke(transform.position, 42f);
        shootSound.Stop();
        shootSound.Play();

        int pellets = 8;
        for(int i = 0; i < pellets; i++)
        {
            Vector3 pelletDirection = direction + UnityEngine.Random.insideUnitSphere * spread;

            RaycastHit hit;
            if (Physics.Raycast(cameraPlayer.position, pelletDirection.normalized, out hit, range, combinedMask))
            {
                StartCoroutine(WhatItHits(hit, pelletDirection));
            }
        }
    }

    private IEnumerator WhatItHits(RaycastHit hit, Vector3 direction)
    {
        if (((1 << hit.collider.gameObject.layer) & Enemy) != 0)
            {
                hit.collider.gameObject.GetComponentInParent<Enemy>().takeDamage(damage);
                yield return null;
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-hit.normal * weaponForce, ForceMode.Impulse);
                StartCoroutine(hit.collider.gameObject.GetComponentInParent<Enemy>().Bleed(-direction, hit.point));
            }
        else if(((1 << hit.collider.gameObject.layer) & Head) != 0)
            {
                hit.collider.gameObject.GetComponentInParent<Enemy>().headShot();
                yield return null;
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(-hit.normal * weaponForce, ForceMode.Impulse);
                StartCoroutine(hit.collider.gameObject.GetComponentInParent<Enemy>().Bleed(-direction, hit.point));
            }
        else if (((1 << hit.collider.gameObject.layer) & Hittable) != 0)
            {
                GameObject spawned = Instantiate(bulletHole, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(-hit.normal, cameraPlayer.up));
                spawned.transform.SetParent(bulletHoleContainer.transform);
                Destroy(spawned, 30f);
            }
    }

    private IEnumerator Flashlight()
    {
        flash.SetActive(true);
        fire.SetActive(true);
        Vector3 currentEuler = fire.transform.localEulerAngles;
        currentEuler.y = UnityEngine.Random.Range(-180, 180);
        fire.transform.localEulerAngles = currentEuler;
        yield return new WaitForSeconds(0.1f);
        flash.SetActive(false);
        fire.SetActive(false);
    }

    private void Reload()
    {
        bulletsText.text = bullets.ToString();
        bulletsAmountText.text = actualBulletsAmount.ToString();
        if (playerController.playerInput.actions["Reload"].triggered && bullets < maxBullets && !reloading && actualBulletsAmount > 0)
        {
            reloading = true;
            canShoot = false;
            anim.SetTrigger(ReloadName); 
        }
    }

    public void FinishReload()
    {
        playerWeapon.GoBack();
        int bulletsLeft =  maxBullets - bullets;
        int bulletsToReload = Mathf.Min(bulletsLeft, actualBulletsAmount);
        bullets += bulletsToReload;
        actualBulletsAmount -= bulletsToReload;

        canShoot = true;
        reloading = false;
    }

    public IEnumerator InitAnim()
    {
        canShoot = false;
        anim.SetTrigger("Opening");
        yield return new WaitForSeconds(0.8f);
        canShoot = true;
    }

    private IEnumerator Delay()
    {
        canShoot = false;
        yield return new WaitForSeconds(cadence);
        canShoot = true;
    }
}
