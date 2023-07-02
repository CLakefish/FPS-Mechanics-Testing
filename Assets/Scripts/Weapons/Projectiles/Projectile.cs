using CUtilities.Weapon;
using System.Collections; using CUtilities.Entity;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ProjectileBase
{
    public Rigidbody rb;
    LayerMask l;
    bool active = true;

    private void Start()
    {
        rb.transform.position = FindObjectOfType<PlayerMovement>().rb.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, baseRayData.speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 0)
        {
            if (baseRayData.hitEnemies.Contains(other.gameObject)) return;

            if (other.gameObject.TryGetComponent<Health>(out Health hp))
            {
                FindObjectOfType<DamageVisualHandler>().DisplayDamage(baseRayData.damage.ToString(), other.ClosestPoint(gameObject.transform.position), Color.white);

                baseRayData.hitEnemies.Add(other.gameObject);

                hp.OnHit(baseRayData.damage);

                if (baseRayData.findNearestTarget)
                {
                    Debug.Log("find");

                    if (CDetection.FindNearest("Enemy", gameObject.transform.position, out Vector3 enemy, baseRayData))
                    {
                        Debug.Log("nuhuh");

                        baseRayData.findNearestTarget = baseRayData.continuousTargetting;

                        StartCoroutine(Check(gameObject.transform.position, enemy, l));
                    }
                }

                if (!baseRayData.canBounce)
                {
                    active = false;
                    Destroy(gameObject);
                }
            }
            else
            {
                active = false;
                Destroy(gameObject);
            }
        }

        if (other.gameObject.layer == 6)
        {
            if (baseRayData.hitReflectors.Contains(other.gameObject))
            {
                 return;
            }

            // Fix this

            baseRayData.hitReflectors.Add(other.gameObject);

            baseRayData.damage *= 2;
            baseRayData.speed *= 2;

            if (baseRayData.findNearestReflect)
            {
                Debug.Log("find");

                if (CDetection.FindNearest("Reflect", gameObject.transform.position, out Vector3 enemy, baseRayData))
                {
                    Debug.Log("nuhuh");

                    baseRayData.findNearestTarget = baseRayData.continuousTargetting;

                    StartCoroutine(Check(gameObject.transform.position, (enemy - gameObject.transform.position).normalized, l));
                }
            }
        }
    }

    public override IEnumerator Check(Vector3 position, Vector3 direction, LayerMask hitLayer)
    {
        l = hitLayer;

        while (active)
        {
            if (gameObject == null) yield break;
            rb.velocity = direction * baseRayData.speed;

            yield return null;
        }
    }
}
