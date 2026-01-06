using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    // Script for managing how the player's weapons work
    public List<WeaponController> starter = new List<WeaponController>();

    public Transform weaponParent;
    public Transform DefaultParent;
    public Transform AimParent;

    public int activeWeaponIndex {get; private set;}

    private WeaponController[] weaponSlots = new WeaponController[2];

    private PlayerController playerController;

    public GameObject ShootPoint;

    public Camera playerCamera;

    public bool isAiming = false;
    public float adsSpeed = 10f;

    private float originalFOV;

    private RigBuilder rigBuilder;

    private WeaponController activeWeapon;

    public Animator anim;

    void Start()
    {
        activeWeaponIndex = -1;

        playerController = GetComponent<PlayerController>();

        playerCamera = GetComponentInChildren<Camera>();

        originalFOV = playerCamera.fieldOfView;

        rigBuilder = GetComponentInChildren<RigBuilder>();

        anim = GetComponent<Animator>();

        foreach (WeaponController weapon in starter)
        {
            AddWeapon();
        }
        
        if (weaponSlots[0] != null)
        {
            SwitchWeapon(0);
        }
    }

    void Update()
    {
        Aim();
        if(isAiming){return;}
        if (playerController.playerInput.actions["FirstWeapon"].triggered)
        {
            SwitchWeapon(0);
        }
        else if (playerController.playerInput.actions["SecondWeapon"].triggered)
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
                                                    AimParent.localPosition,
                                                    Time.deltaTime * adsSpeed
                                                );
            weaponParent.localRotation = AimParent.localRotation;
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

        weaponSlots[index].gameObject.SetActive(true);
        activeWeaponIndex = index;
        
        Debug.Log($"Arma cambiada a slot {index}");

        rigBuilder.Build();
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
}
