using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    // Script for managing how the player's weapons work
    [Header("Armas en posesion")]
    public List<WeaponController> starter = new List<WeaponController>();

    public Transform weaponParent;
    public Transform DefaultParent;
    public Transform AimParent;
    public TwoBoneIKConstraint rightHandIK;
    public TwoBoneIKConstraint leftHandIK;
    public string GripR = "GripR";
    public string GripL = "GripL";
    public int activeWeaponIndex {get; private set;}

    private WeaponController[] weaponSlots = new WeaponController[3];

    public HUD playerHud;
    private PlayerController playerController;

    public GameObject ShootPoint;

    public Camera playerCamera;

    public bool isAiming = false;
    public float adsSpeed = 10f;

    private float originalFOV;

    private RigBuilder rigBuilder;

    public WeaponController activeWeapon;
    public Vector3 desviacion = Vector3.zero;

    public Animator anim;

    [Header("Granadas")]
    [HideInInspector] public InputAction granadeAction;
    public GameObject granade;
    public float trowForce = 10;
    public float maxGranadeCap = 5;
    public float actualGranades;

    void Start()
    {
        activeWeaponIndex = -1;
        playerController = GetComponent<PlayerController>();
        playerCamera = GetComponentInChildren<Camera>();
        originalFOV = playerCamera.fieldOfView;
        rigBuilder = GetComponentInChildren<RigBuilder>();
        anim = GetComponent<Animator>();
        playerHud = GetComponent<HUD>();
        actualGranades = maxGranadeCap;
        granadeAction = playerController.playerInput.actions["Granade"];

        AddWeapon();
        
        if (weaponSlots[0] != null)
        {
            SwitchWeapon(0);
        }
    }

    void Update()
    {
        Aim();
        TrowGranade();
        if(isAiming || activeWeapon.reloading || activeWeapon.canShoot == false){return;}

        if (playerController.playerInput.actions["FirstWeapon"].triggered)
        {
            SwitchWeapon(0);
        }
        else if (playerController.playerInput.actions["SecondWeapon"].triggered)
        {
            SwitchWeapon(1);
        }
        else if (playerController.playerInput.actions["ThirdWeapon"].triggered)
        {
            SwitchWeapon(2);
        }
    }

    private void Aim()
    {
        if (playerController.playerInput.actions["Aim"].IsPressed())
        {
            isAiming = true;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, originalFOV - 20f, Time.deltaTime * adsSpeed);
            ShootPoint.SetActive(false);
            weaponParent.localPosition = Vector3.Lerp(
                                                    weaponParent.localPosition,
                                                    AimParent.localPosition + activeWeapon.desviacion,
                                                    Time.deltaTime * adsSpeed
                                                );
            weaponParent.localRotation = AimParent.localRotation;

            if (playerController.isCrouching)
            {
                playerController.velocity = 6.0f;
            }
            else
            {
                playerController.velocity = 8.5f;
            }
            
        }
        else
        {
            isAiming = false;
            if(!playerController.isSprinting)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, originalFOV, Time.deltaTime * adsSpeed);
            }
            ShootPoint.SetActive(true);
            weaponParent.localPosition = Vector3.Lerp(
                                                    weaponParent.localPosition,
                                                    DefaultParent.localPosition,
                                                    Time.deltaTime * adsSpeed
                                                );
            weaponParent.localRotation = DefaultParent.localRotation;
            playerController.velocity = playerController.actualVelocity;
        }
    }

    private void AddWeapon()
    {

        int slotIndex = 0;

            foreach (Transform child in weaponParent)
            {
                if (slotIndex >= weaponSlots.Length)
                    break;

                WeaponController weapon = child.GetComponent<WeaponController>();
                if (weapon == null)
                    continue;
                weaponParent.position = DefaultParent.position; 
                weaponParent.rotation = DefaultParent.rotation; 
                weaponParent.localScale = DefaultParent.localScale;

                weaponSlots[slotIndex] = weapon;
                weapon.gameObject.SetActive(false);

                slotIndex++;
            }
    }

    private void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weaponSlots.Length || weaponSlots[index] == null)
        {
            return;
        }

        if (activeWeaponIndex >= 0 && weaponSlots[activeWeaponIndex] != null)
        {
            weaponSlots[activeWeaponIndex].gameObject.SetActive(false);
        }
        weaponParent.position = DefaultParent.position;
        weaponParent.rotation = DefaultParent.rotation;
        weaponParent.localScale = DefaultParent.localScale;
        playerHud.ChangeIcon(index);
        WeaponController weapon = weaponSlots[index]; 
        weapon.gameObject.SetActive(true);
        AssignIK(weapon);
        activeWeaponIndex = index;

        rigBuilder.Build();
        activeWeapon = weapon;
        activeWeapon.canShoot = true;
        activeWeapon.initAnim();
    }

    public void GoBack()
    {
        weaponParent.localPosition = Vector3.Lerp(
                                                    weaponParent.localPosition,
                                                    DefaultParent.localPosition,
                                                    Time.deltaTime * adsSpeed
                                                );
        weaponParent.localRotation = DefaultParent.localRotation;
    }

    public void TrowGranade()
    {
        if (granadeAction.WasPressedThisFrame() && actualGranades > 0)
        {
            GameObject newGranade = Instantiate(granade, playerCamera.transform.position, playerCamera.transform.rotation);
            newGranade.GetComponent<Rigidbody>().AddForce(playerCamera.transform.forward * trowForce);
            actualGranades -= 1;
        }
    }

    public void AssignIK(WeaponController weapon)
    {
        Transform rightTarget = weapon.transform.Find(GripR);
        Transform leftTarget = weapon.transform.Find(GripL);

        rightHandIK.data.target = rightTarget;
        leftHandIK.data.target  = leftTarget;    
    }
}
