using System.Collections;
using System.Collections.Generic; using CUtilities.Entity; using CUtilities.Weapon;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Position")]
    public Transform weaponPosition;

    [Header("Weapons")]
    public List<GameObject> visuals;
    public List<GameObject> instantiated;
    public List<WeaponObject> heldObjects;
    public List<int> heldObjectAmmoCounts;
    public int selectedIndex;

    [Header("Held Weapon")]
    public WeaponObject currentHeldWeaponObject;
    public WeaponBase currentHeldWeaponBase;
    public GameObject currentHeldGameObject;

    [Header("Enabling")]
    [SerializeField] public bool canShoot = true;

    private void Start()
    {
        if (heldObjects.Count <= 0) return;

        selectedIndex = 0;
        heldObjectAmmoCounts.Add(0);
        SwapWeapon(heldObjects[0], true);
    }

    public void SwapWeapon(WeaponObject newWeapon, bool onStart = false)
    {
        if (!onStart && heldObjects.Count <= 1) return;
        if (currentHeldWeaponBase != null) currentHeldWeaponBase.OnUnequip();

        currentHeldWeaponObject = newWeapon;

        if (!visuals.Contains(currentHeldWeaponObject.weaponPrefab))
        {
            currentHeldGameObject = Instantiate(currentHeldWeaponObject.weaponPrefab, weaponPosition);
            visuals.Add(currentHeldWeaponObject.weaponPrefab);
            instantiated.Add(currentHeldGameObject);
        }
        else currentHeldGameObject = instantiated[visuals.IndexOf(currentHeldWeaponObject.weaponPrefab)].gameObject;

        currentHeldWeaponBase = currentHeldGameObject.GetComponentInChildren<WeaponBase>();
        currentHeldWeaponBase.currentAmmo = heldObjectAmmoCounts[selectedIndex];

        if (currentHeldWeaponBase != null)
        {
            currentHeldWeaponBase.OnAttach(weaponPosition.transform.position);
            currentHeldWeaponBase.OnEquip();
        }
    }

    private void Update()
    {
        if (currentHeldWeaponBase == null || !canShoot) return;

        // Ensure the ammo is kept up to date
        heldObjectAmmoCounts[selectedIndex] = currentHeldWeaponBase.currentAmmo;

        // Inputs
        if (currentHeldWeaponBase.fireType == WeaponFireType.Single)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                currentHeldWeaponBase.Shoot();
        }
        else
        {
            if (Input.GetMouseButton(0))
                currentHeldWeaponBase.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
            currentHeldWeaponBase.Reload();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            currentHeldWeaponBase.Special();

        // Swap Weapon
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedIndex < heldObjects.Count - 1) selectedIndex++;
            else selectedIndex = 0;

            SwapWeapon(heldObjects[selectedIndex]);
        }
    }
}
