using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    [Header("ObjToFollow")]
    [SerializeField] Transform Object;

    private void Update()
    {
        transform.position = Object.transform.position;
    }
}
