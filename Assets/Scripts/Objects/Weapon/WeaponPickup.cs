using CUtilities.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponObject weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerWeapons obj = other.GetComponent<PlayerWeapons>();

            obj.heldObjects.Add(weapon);
            obj.heldObjectAmmoCounts.Add(weapon.weaponPrefab.GetComponent<WeaponBase>().maxAmmo);
            obj.selectedIndex = obj.heldObjects.Count - 1;

            obj.SwapWeapon(obj.heldObjects[obj.heldObjects.Count - 1]);

            Destroy(gameObject);
        }
    }
}
