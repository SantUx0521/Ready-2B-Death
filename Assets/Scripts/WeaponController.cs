using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Using as a universal base for weapons, so i can manage damage, recoil, range, etc from their own scripts
    [Header("Layers")]
    public LayerMask Hittable;
    public LayerMask Enemy;
    [SerializeField] GameObject flash;
    [SerializeField] GameObject fire;
    [HideInInspector] public float range;
    [HideInInspector] public int damage;
    [HideInInspector] public float spread;

    CameraShake cameraShake;

    public bool canShoot = true;
    private PlayerController playerController;
    private Animator anim;
    private Transform cameraPlayer;
    void Start()
    {
        cameraPlayer = GameObject.FindWithTag("MainCamera").transform;
        playerController = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
        cameraShake = cameraPlayer.GetComponent<CameraShake>();
    }

    void Update()
    {
        Shoot();
        Debug.DrawRay(cameraPlayer.position, cameraPlayer.forward * range, Color.red);
    }

    private void Shoot()
    {
        if (playerController.playerInput.actions["Fire"].triggered && canShoot)
        {
            anim.SetTrigger("Shoot");
            StartCoroutine(Flashlight());
            StartCoroutine(cameraShake.Shake(0.05f, 0.05f));
            BulletHit();
            StartCoroutine(Delay());
        }
    }

    private void BulletHit()
    {
        LayerMask combinedMask = Enemy | Hittable;
        Vector3 direction = cameraPlayer.forward;
        direction = Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0) * direction;

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.position, direction, out hit, range, combinedMask))
        {
            if (((1 << hit.collider.gameObject.layer) & Enemy) != 0)
            {
                hit.collider.gameObject.GetComponent<Enemy>().takeDamage(damage);
                return;
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

    private IEnumerator Delay()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.2f);
        canShoot = true;
    }
}
