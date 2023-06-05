using System.Collections;
using System.Collections.Generic; using CUtilities.Weapon; using CUtilities.Entity;
using UnityEngine;

public class Hitscan : ProjectileBase
{
    [Header("BulletHoles")]
    [SerializeField] List<GameObject> bulletHoles;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public override IEnumerator Check(Vector3 position, Vector3 direction, LayerMask hitLayer)
    {
        yield return new WaitForEndOfFrame();

        if (Physics.Raycast(position, direction, out RaycastHit info, 10000, hitLayer))
        {
            yield return new WaitForEndOfFrame();

            if (info.collider.gameObject.layer == 0)
            {
                Debug.Log(0);

                if (baseRayData.hitEnemies.Contains(info.collider.gameObject))
                {
                    yield return Check(info.point, direction, hitLayer);

                    yield break;
                }

                GameObject newObj = Instantiate(bulletHoles[Random.Range(0, bulletHoles.Count - 1)], info.point, Quaternion.identity);
                Destroy(newObj, 0.75f);

                if (info.collider.gameObject.TryGetComponent(out Health hitObject))
                {
                    FindObjectOfType<DamageVisualHandler>().DisplayDamage(baseRayData.damage.ToString(), info.point, Color.white);

                    baseRayData.hitEnemies.Add(info.collider.gameObject);

                    hitObject.OnHit(baseRayData.damage);
                }

                if (baseRayData.findNearestTarget)
                {
                    Debug.Log("find");

                    if (CDetection.FindNearest("Enemy", info.point, out Vector3 enemy, baseRayData))
                    {
                        Debug.Log("nuhuh");

                        baseRayData.findNearestTarget = baseRayData.continuousTargetting;

                        yield return Check(info.point, enemy, hitLayer);

                        yield break;
                    }

                    enemy = Vector3.Reflect(direction, info.point);

                    baseRayData.findNearestTarget = false;


                    yield return Check(info.point, enemy, hitLayer);

                    yield break;
                }

                if (baseRayData.canBounce)
                {
                    switch (baseRayData.bounceCount)
                    {
                        case (>= 1):

                            Vector3 reflectedAngle = Vector3.Reflect(direction, info.normal);

                            Debug.DrawLine(info.point, reflectedAngle, Color.red, 2f);

                            baseRayData.bounceCount--;

                            yield return Check(info.point, reflectedAngle, hitLayer);

                            break;

                        case (<= 0):

                            break;
                    }
                }
            }
            if (info.collider.gameObject.layer == 6)
            {
                if (baseRayData.hitReflectors.Contains(info.collider.gameObject))
                {
                    yield return Check(info.point, direction, hitLayer);

                    yield break;
                }

                // Fix this

                baseRayData.hitReflectors.Add(info.collider.gameObject);

                baseRayData.damage *= 2;

                FindObjectOfType<DamageVisualHandler>().DisplayDamage("x2!", info.point, Color.red);

                if (info.collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody coinRb))
                {
                    coinRb.velocity = new Vector3(0f, 0f, 0f);
                    coinRb.AddForce(Vector3.up * 3f, ForceMode.VelocityChange);
                }

                if (baseRayData.findNearestReflect)
                {
                    if (CDetection.FindNearest("Reflect", info.point, out Vector3 reflect, baseRayData))
                    {
                        // Debug.Log(enemyDir);//

                        yield return new WaitForEndOfFrame();

                        yield return Check(info.point, reflect, hitLayer);

                        Destroy(info.collider);

                        yield break;
                    }
                    else
                    {

                        baseRayData.findNearestTarget = baseRayData.continuousTargetting;
                        yield return null;
                    }
                }

                if (CDetection.FindNearest("Enemy", info.point, out Vector3 enemyDir, baseRayData))
                {
                    yield return Check(info.point, enemyDir, hitLayer);

                    Destroy(info.collider);

                    yield break;
                }

                enemyDir = Vector3.Reflect(direction, info.normal);
                baseRayData.findNearestTarget = false;

                yield return Check(info.point, enemyDir, hitLayer);

                Destroy(info.collider);

                yield break;
            }
        }
    }
}
