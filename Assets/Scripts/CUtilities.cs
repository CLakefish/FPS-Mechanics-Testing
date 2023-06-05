using System; using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUtilities
{
    // Player utilities
    namespace Player
    {
        public enum PlayerStates
        {
            Grounded,
            Jumping,
            Falling,
            Sliding,
        }

        public static class CMove
        {
            public static IEnumerator LerpSpeed(float moveSpeed, float desiredMoveSpeed, float lastDesiredMoveSpeed)
            {
                lastDesiredMoveSpeed = desiredMoveSpeed;

                float time = 0;
                float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
                float startVal = moveSpeed;

                while (time < difference)
                {
                    moveSpeed = Mathf.Lerp(startVal, desiredMoveSpeed, time / difference);
                    time += Time.deltaTime;
                    yield return null;
                }

                moveSpeed = desiredMoveSpeed;
            }
        }

        public static class CSlopeDetection
        {

        }
    }

    // Any entity utilities
    namespace Entity
    {
        using CUtilities.Weapon;

        public static class CDetection
        {
            public static Transform NearestObj(GameObject[] objs, Vector3 position)
            {
                Transform closestTarget = null;
                float closestDistance = Mathf.Infinity;
                Vector3 currentPos = position;

                for (int i = 0; i < objs.Length; i++)
                {
                    Vector3 dirToTarget = objs[i].transform.position - currentPos;
                    float distance = dirToTarget.sqrMagnitude;

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = objs[i].transform;
                    }
                }

                return closestTarget;
            }

            public static bool FindNearest(string tag, Vector3 point, out Vector3 direction, RaycastData baseRayData)
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

        public abstract class Health : MonoBehaviour
        {
            [Header("Health Values")]
            [SerializeField] int currentHP;
            public int CurrentHP
            {
                get { return currentHP; }
                set
                {
                    currentHP = value;
                    currentHP = Mathf.Clamp(currentHP, 0, maxHP);
                }
            }

            [SerializeField] int maxHP;
            public int MaxHP
            {
                get { return maxHP; }
                set { maxHP = value; }
            }

            [Header("Invulnerability")]
            [SerializeField] public bool isInvulnerable;
            [SerializeField] float invulnerabilitySeconds;

            public IEnumerator Invulnerable()
            {
                if (isInvulnerable) yield break;

                isInvulnerable = true;

                yield return new WaitForSeconds(invulnerabilitySeconds);

                isInvulnerable = false;
            }

            public void OnHit(int damage)
            {
                if (isInvulnerable) return;

                if (currentHP - damage <= 0) OnDeath();
                else OnDamage(damage);
            }

            public abstract void OnDamage(int damage);

            public abstract void OnDeath();

            public abstract void OnGainHealth(int health);
        }
    }

    namespace Weapon
    {
        public abstract class WeaponBase : MonoBehaviour
        {
            public int currentAmmo;
            public int maxAmmo;

            public GameObject projectile;
            public WeaponFireType fireType;

            public abstract void Shoot();
            public abstract void Reload();

            public abstract void Special();

            public abstract void OnEquip();
            public abstract void OnUnequip();
            public abstract void OnAttach(Vector3 position);
        }

        public abstract class ProjectileBase : MonoBehaviour
        {
            internal RaycastData baseRayData;
            public abstract IEnumerator Check(Vector3 position, Vector3 direction, LayerMask hitLayer);
        }

        [Serializable]
        public struct RaycastData
        {
            public int bounceCount;
            public int damage;

            public float speed;

            internal bool findNearestTarget;
            public bool continuousTargetting;
            public bool canBounce;
            public bool findNearestReflect;

            public List<GameObject> hitEnemies,
                                    hitReflectors;

            public void Values(RaycastData te)
            {
                bounceCount = te.bounceCount;
                speed = te.speed;
                findNearestTarget = te.findNearestTarget;
                canBounce = te.canBounce;
                continuousTargetting = te.continuousTargetting;
                findNearestReflect = te.findNearestReflect;

                hitEnemies = new List<GameObject>();
                hitReflectors = new List<GameObject>();
            }
        }
        public enum WeaponFireType
        {
            Single,
            Multi,
        }
    }

    namespace Object
    {
        public interface IInteractible
        {
            public void Interact();
            public string GetString();
        }
    }
}
