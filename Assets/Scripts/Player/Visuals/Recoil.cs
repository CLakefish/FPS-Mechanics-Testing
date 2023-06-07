using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Rotations")]
    private Vector3 currentRotation,
                    targetRotation;

    [Header("Movement Values")]
    private float snapping,
                  returnSpeed;

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapping * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void AddRecoil(Vector3 recoil, float snap, float originate)
    {
        snapping = snap;

        returnSpeed = originate;

        targetRotation += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
    }
}
