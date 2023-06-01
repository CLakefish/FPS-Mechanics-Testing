using System.Collections; using CUtilities.Weapon; 
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody rb;
    PlayerMovement player;

    float slideX = 100f,
          slideLastX;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>();

        //rb.velocity = player.rb.velocity;

        rb.AddForce(Vector3.up * 10 + (Camera.main.transform.forward.normalized * 2f), ForceMode.VelocityChange);

        Destroy(gameObject, 2f);
    }

}
