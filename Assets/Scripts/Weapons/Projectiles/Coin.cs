using System.Collections; using CUtilities.Weapon; 
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody rb;
    PlayerMovement player;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerMovement>();

        rb.velocity = new Vector3(player.rb.velocity.x, 0f, player.rb.velocity.z);

        rb.AddForce(Vector3.up * 8 + (Camera.main.transform.forward.normalized * 3f), ForceMode.VelocityChange);

        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(Camera.main.transform.rotation.x, 0f, Camera.main.transform.rotation.z), Space.Self);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10f);
    }

}
