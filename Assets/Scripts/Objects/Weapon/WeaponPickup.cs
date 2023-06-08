using CUtilities.Weapon;
using System.Collections;
using System.Collections.Generic; using CUtilities.Object;
using UnityEngine;

public class WeaponPickup : Interactable
{
    [Header("Weapon to provide")]
    [SerializeField] public WeaponObject weapon;

    public override string GetDescription()
    {
        return description;
    }

    public override void Interact()
    {
        PlayerWeapons obj = FindObjectOfType<PlayerWeapons>();

        obj.heldObjects.Add(weapon);
        obj.heldObjectAmmoCounts.Add(weapon.weaponPrefab.GetComponent<WeaponBase>().maxAmmo);
        obj.selectedIndex = obj.heldObjects.Count - 1;

        obj.SwapWeapon(obj.heldObjects[obj.heldObjects.Count - 1]);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerWeapons obj = other.GetComponentInParent<PlayerWeapons>();

            obj.heldObjects.Add(weapon);
            obj.heldObjectAmmoCounts.Add(weapon.weaponPrefab.GetComponent<WeaponBase>().maxAmmo);
            obj.selectedIndex = obj.heldObjects.Count - 1;

            obj.SwapWeapon(obj.heldObjects[obj.heldObjects.Count - 1]);

            Destroy(gameObject);
        }
    }
}
