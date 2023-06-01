using System.Collections;
using System.Collections.Generic; using CUtilities.Weapon; using CUtilities.Entity;
using UnityEngine;

public class Hitscan : ProjectileBase
{
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

                if (info.collider.gameObject.TryGetComponent(out Health hitObject))
                {
                    FindObjectOfType<DamageVisualHandler>().DisplayDamage(baseRayData.damage.ToString(), info.point, Color.white);

                    baseRayData.hitEnemies.Add(info.collider.gameObject);

                    hitObject.OnHit(baseRayData.damage);
                }

                if (baseRayData.findNearestTarget)
                {
                    Debug.Log("find");

                    if (FindNearest("Enemy", info.point, out Vector3 enemy))
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

                if (info.collider.gameObject.GetComponent<Rigidbody>() != null)
                {
                    info.collider.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                }

                baseRayData.hitReflectors.Add(info.collider.gameObject);

                baseRayData.damage *= 2;

                //FindObjectOfType<DamageVisualHandler>().DisplayDamage("no", info.point, Color.red);


                if (baseRayData.findNearestReflect)
                {
                    if (FindNearest("Reflect", info.point, out Vector3 reflect))
                    {
                        // Debug.Log(enemyDir);//

                        yield return new WaitForEndOfFrame();

                        yield return Check(info.point, reflect, hitLayer);

                        yield break;
                    }
                    else
                    {

                        baseRayData.findNearestTarget = baseRayData.continuousTargetting;
                        yield return null;
                    }
                }

                if (FindNearest("Enemy", info.point, out Vector3 enemyDir))
                {
                    yield return Check(info.point, enemyDir, hitLayer);

                    yield break;
                }

                enemyDir = Vector3.Reflect(direction, info.normal);
                baseRayData.findNearestTarget = false;

                yield return Check(info.point, enemyDir, hitLayer);

                yield break;
            }
        }
    }

    bool FindNearest(string tag, Vector3 point, out Vector3 direction)
    {
        Vector3 projectileDirection = new();

        List<GameObject> objs = new();

        if (tag == "Reflect")
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                if (baseRayData.hitReflectors.Contains(obj) == false)
                {

                    objs.Add(obj);
                }
            }
        }

        if (tag == "Enemy")
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
            {
                if (baseRayData.hitEnemies.Contains(obj) == false)
                {

                    objs.Add(obj);
                }
            }
        }

        foreach (GameObject obj in objs)
        {
            Debug.DrawRay(obj.transform.position, Vector3.up, Color.green, 2f);
        }

        if (objs.Count >= 1)
        {
            GameObject nearestObject = CDetection.NearestObj(objs.ToArray(), point).gameObject;
            if (nearestObject == null)
            {
                direction = default;
                return false;
            }

            if (Vector3.Distance(nearestObject.transform.position, point) <= 100)
            {
                projectileDirection = (nearestObject.transform.position - point).normalized;
            }

            direction = projectileDirection;

            return true;
        }
        else
        {
            direction = default;
            return false;
        }
    }
}
