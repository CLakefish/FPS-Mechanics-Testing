using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Orientation")]
    [SerializeField] public Transform playerObj;

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

    float mouseRotation;
    internal Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        // FOV Change based on FOV 
        Camera.main.fieldOfView = fieldOfView;


        // https://github.com/deadlykam/TutorialFPSRotation/tree/main/TutorialFPSRotation/Assets/TutorialFPSRotation/Scripts

        mousePos = new Vector2(Input.GetAxis("Mouse X") * sensitivity.x, Input.GetAxis("Mouse Y") * sensitivity.y);

        mouseRotation -= mousePos.y;
        mouseRotation = Mathf.Clamp(mouseRotation, -89f, 89f);

        transform.localRotation = Quaternion.Euler(mouseRotation, 0f, 0f);
        playerObj.Rotate(Vector3.up * mousePos.x);
    }
}
