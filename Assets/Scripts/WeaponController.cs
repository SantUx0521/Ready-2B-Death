using Unity.InferenceEngine;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // Using as a universal base for weapons, so i can manage damage, recoil, range, etc from their own scripts
    [Header("Layers")]
    public LayerMask Hittable;
    public LayerMask Enemy;

    [HideInInspector] public float range;
    [HideInInspector] public float damage;
    private PlayerController playerController;
    private Transform cameraPlayer;
    void Start()
    {
        cameraPlayer = GameObject.FindWithTag("MainCamera").transform;
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        Shoot();
        Debug.DrawRay(cameraPlayer.position, cameraPlayer.forward * range, Color.red);
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (playerController.playerInput.actions["Fire"].triggered)
        {
            LayerMask combinedMask = Enemy | Hittable;
            if (Physics.Raycast(cameraPlayer.position, cameraPlayer.forward, out hit, range, combinedMask))
            {
                if (((1 << hit.collider.gameObject.layer) & Enemy) != 0)
                {
                    Debug.Log("Enemy Hitted");
                    return;
                }

                if (((1 << hit.collider.gameObject.layer) & Hittable) != 0)
                {
                    Debug.Log("Hit");
                    return;
                }
            }
        }
    }
}
