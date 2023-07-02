using System.Collections;
using System.Collections.Generic; using CUtilities.Weapon; using CUtilities.Entity;
using UnityEngine;

public class Basic : WeaponBase
{
    [Header("Reference")]
    PlayerMovement player;

    [Header("Mesh")]
    public MeshRenderer mesh;

    [Header("Hittable Layers")]
    public LayerMask hitLayer;

    [Header("Special")]
    public Coin coin;

    [Header("Projectile Variables")]
    public RaycastData baseRayData;
    public int targetMaxDistance;
    bool canShoot = true;

    [Header("Recoil Parameters")]
    Recoil rS;
    public Vector3 recoil;
    public float recoilSnapping;
    public float recoilEndSpeed;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        rS = FindObjectOfType<Recoil>();
    }

    public override void Reload()
    {
        if (canShoot) StartCoroutine(reloadEffects());
    }

    IEnumerator reloadEffects()
    {
        canShoot = false;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        canShoot = true;
    }


    public override void Shoot()
    {
        if (currentAmmo <= 1)
        {
            currentAmmo = 0;
            Reload();
            return;
        }

        if (Time.time >= fireRate + previousFireTime && currentAmmo != 0 && canShoot)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                currentAmmo--;
                Debug.Log(currentAmmo);

                // Raycast to Mouse Input Position in world
                Ray ray = new Ray(player.rb.transform.position, (player.GetViewDirection() + new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), -Random.Range(-bulletSpread, bulletSpread))).normalized);

                ProjectileBase proj = Instantiate(projectile).GetComponent<ProjectileBase>();

                RaycastData data = new();
                data.Values(baseRayData);

                proj.baseRayData = data;
                proj.baseRayData.damage = baseRayData.damage;

                StartCoroutine(proj.Check(player.rb.transform.position, ray.direction, hitLayer));
            }

            rS.AddRecoil(recoil, recoilSnapping, recoilEndSpeed);

            previousFireTime = Time.time;
        }
        else return;
    }

    public override void Special()
    {
        Coin theCoin = Instantiate(coin, transform.position, player.rb.transform.rotation);
    }

    public override void OnEquip()
    {
        mesh.enabled = true;
    }

    public override void OnUnequip()
    {
        mesh.enabled = false;
    }

    public override void OnAttach(Vector3 position)
    {
        transform.position = position;
    }
}
