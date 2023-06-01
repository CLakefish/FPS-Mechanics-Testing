using System.Collections;
using System.Collections.Generic; using CUtilities.Weapon; using CUtilities.Entity;
using UnityEngine;

public class Basic : WeaponBase
{
    [Header("Reference")]
    PlayerMovement player;

    [Header("Hittable Layers")]
    public LayerMask hitLayer;

    [Header("Coin")]
    public Coin coin;

    [Header("Weapon Values")]
    public float fireRate;
    public float bulletsPerShot;
    public float bulletSpread;
    public float reloadTime;
    public Vector3 recoil;
    public float recoilSnapping;
    public float recoilEndSpeed;
    float previousFireTime;

    [Header("Trail Renderer")]
    public bool raycast;
    public bool visualize;
    public TrailRenderer vfxTrail;
    public MeshRenderer mesh;
    //public float speed;

    [Header("Variables")]
    public RaycastData baseRayData;
    public bool continuousTargetting;
    public int targetMaxDistance;
    bool canShoot = true;
    //Recoil rS;


    public float mouseValue = 0.01f;
    public float maxDistance = 0.06f;
    public float mouseRotation = 4f;
    public float maxRotation = 5f;
    public float smoothing = 12f;
    public float smoothingPosition = 10f;
    Vector3 sway;
    Vector3 swayPosition;

    const float hitCorrection = 2000f;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        //rS = FindObjectOfType<Recoil>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sway();
    }

    void SwayMath()
    {
        Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (lookInput.x == 0 || lookInput.y == 0) return;

        Vector3 invertedP = lookInput * -mouseValue;
        Vector3 invertedR = lookInput * -mouseRotation;

        invertedP.x = Mathf.Clamp(invertedP.x, -maxDistance, maxDistance);
        invertedP.y = Mathf.Clamp(invertedP.y, -maxDistance, maxDistance);

        invertedR.x = Mathf.Clamp(invertedR.x, -maxRotation, maxRotation);
        invertedR.y = Mathf.Clamp(invertedR.y, -maxRotation, maxRotation);

        swayPosition = invertedP;
        sway = new Vector3(invertedR.y, invertedR.x, invertedR.x);
    }

    void Sway()
    {
        SwayMath();

        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPosition, Time.deltaTime * smoothingPosition);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(sway), Time.deltaTime * smoothing);
    }

    public override void Reload()
    {
        if (canShoot) StartCoroutine(reloadEffects());
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

                StartCoroutine(proj.Check(player.rb.transform.position, ray.direction * 10000, hitLayer));
            }

            //rS.AddRecoil(recoil, recoilSnapping, recoilEndSpeed);

            previousFireTime = Time.time;
        }
        else
        {
            return;
        }
    }

    IEnumerator reloadEffects()
    {
        canShoot = false;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        canShoot = true;
    }

    public override void OnEquip()
    {
        mesh.enabled = true;
        //throw new System.NotImplementedException();
    }

    public override void OnUnequip()
    {
        mesh.enabled = false;
        //throw new System.NotImplementedException();
    }

    public override void OnAttach(Vector3 position)
    {
        transform.position = position;
        //throw new System.NotImplementedException();
    }

    public override void Special()
    {
        Coin theCoin = Instantiate(coin, transform.position, Quaternion.identity);

        //throw new System.NotImplementedException();
    }
}
