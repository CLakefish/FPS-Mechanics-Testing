using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Orientation")]
    [SerializeField] public Transform lookRotation;
    [SerializeField] public Transform playerObj;
    internal PlayerMovement playerMovement;
    internal Camera cam;

    [Header("Mouse Stuff")]
    [SerializeField] public bool canLook = true;
    [SerializeField]
    public Vector2 Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [SerializeField] Vector2 sensitivity;

    [SerializeField]
    public float FOV
    {
        get { return fieldOfView; }
        set { fieldOfView = value; }
    }
    [SerializeField] float fieldOfView;

    float shakePos;
    public bool invertY;
    public bool invertX;

    Vector2 mouseRotation;
    internal Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        cam = GetComponent<Camera>();

        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // FOV Change based on FOV 
        cam.fieldOfView = fieldOfView;

        // Get mouse pos
        mousePos = new Vector2(Input.GetAxis("Mouse X") * sensitivity.x, Input.GetAxis("Mouse Y") * sensitivity.y);

        // Mouse stuff
        mouseRotation.x = (invertY) ? mouseRotation.x += mousePos.y : mouseRotation.x -= mousePos.y;
        mouseRotation.y = (invertX) ? mouseRotation.y -= mousePos.x : mouseRotation.y += mousePos.x;

        // Clamp
        mouseRotation.x = Mathf.Clamp(mouseRotation.x, -90f, 90f);

        // Proper Rotation
        Vector3 direction = new Vector3(mouseRotation.x - playerMovement.viewTilt.y, mouseRotation.y, transform.rotation.z + playerMovement.viewTilt.x + shakePos);

        transform.rotation = Quaternion.Euler(direction);
        lookRotation.rotation = Quaternion.Euler(new Vector3(0, direction.y, direction.z));

        // orientation.rotation = Quaternion.Euler(0f, mouseRotation.y, 0f);

        // Proper Positioning
        transform.position = new Vector3(transform.position.x, playerObj.transform.position.y, transform.position.z);
    }
}
