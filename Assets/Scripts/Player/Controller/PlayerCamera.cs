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
    public bool invertY;
    public bool invertX;

    Vector2 mouseRotation;
    internal Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // FOV Change based on FOV 
        Camera.main.fieldOfView = fieldOfView;

        // Invert Camera
        invertX = PlayerPrefs.GetInt("InvertX") == 1;
        invertY = PlayerPrefs.GetInt("InvertY") == 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (!canLook) return;

        // https://github.com/deadlykam/TutorialFPSRotation/tree/main/TutorialFPSRotation/Assets/TutorialFPSRotation/Scripts
        Camera.main.fieldOfView = fieldOfView;

        mousePos = new Vector2(Input.GetAxis("Mouse X") * sensitivity.x, Input.GetAxis("Mouse Y") * sensitivity.y);

        // I'm no good with Quaternions so this'll do lmao
        if (invertX) mousePos = new Vector2(-mousePos.x, mousePos.y);
        if (invertY) mousePos = new Vector2(mousePos.x, -mousePos.y);

        mouseRotation.x -= mousePos.y;
        mouseRotation.y += mousePos.x;
        mouseRotation.x = Mathf.Clamp(mouseRotation.x, -89f, 89f);

        Vector3 direction = new Vector3(mouseRotation.x, mouseRotation.y, transform.rotation.z);
        transform.localRotation = Quaternion.Euler(direction);
    }

    public void InvertX()
    {
        invertX = !invertX;
        PlayerPrefs.SetInt("InvertX", invertX == true ? 1 : 0);
    }

    public void InvertY()
    {
        invertY = !invertY;
        PlayerPrefs.SetInt("InvertY", invertY == true ? 1 : 0);
    }
}
