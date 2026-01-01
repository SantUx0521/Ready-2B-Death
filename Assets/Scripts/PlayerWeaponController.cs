using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        activeWeaponIndex = -1;

        playerController = GetComponent<PlayerController>();

        foreach (WeaponController weapon in starter)
        {
            AddWeapon(weapon);
        }
        
        if (weaponSlots[0] != null)
        {
            SwitchWeapon(0);
        }
    }

    void Update()
    {
        if (playerController.playerInput.actions["FirstWeapon"].triggered)
        {
            SwitchWeapon(0);
        }
        else if (playerController.playerInput.actions["SecondWeapon"].triggered)
        {
            SwitchWeapon(2);
        }
    }

    private void AddWeapon(WeaponController weapon)
    {

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if(weaponSlots[i] == null)
            {
                weaponParent.position = DefaultParent.position;
                weaponParent.rotation = DefaultParent.rotation;
                weaponParent.localScale = DefaultParent.localScale;
                WeaponController weaponClone = Instantiate(weapon, weaponParent);
                weaponClone.gameObject.SetActive(false);

                weaponSlots[i] = weaponClone;
                return;
            }
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
    }
}
