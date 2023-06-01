using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CUtilities/Weapon Objects")]
public class WeaponObject : ScriptableObject
{
    public GameObject weaponPrefab;
    public RuntimeAnimatorController weaponAnimationRig;
}
