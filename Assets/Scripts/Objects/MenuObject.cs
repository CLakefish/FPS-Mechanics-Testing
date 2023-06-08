using System.Collections;
using System.Collections.Generic; using CUtilities.Object; using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuObject : Interactable
{
    #region Parameters

    [Header("References")]
    PlayerInteract player;
    PlayerWeapons playerWeapons;
    PlayerMovement playerMovement;
    PlayerCamera playerCamera;

    [Header("Movement Parameters")]
    const float cameraPosSpeed = 5f,
                cameraRotateSpeed = 10f;

    [Header("Camera Position")]
    [SerializeField] Transform cameraPosition;
    Transform previousPosition;

    [Header("Player Position")]
    [SerializeField] Transform playerPosition;

    #endregion

    #region Variables

    [Header("Gameplay Variables")]
    [SerializeField] bool openOnStart = false;

    [Header("Debugging")]
    [SerializeField] bool playerActivated = false;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerInteract>();
        playerWeapons = player.GetComponent<PlayerWeapons>();

        playerMovement = player.GetComponent<PlayerMovement>();
        playerCamera = player.GetComponentInChildren<PlayerCamera>();

        previousPosition = FindObjectOfType<Recoil>().gameObject.transform; // Since the recoil object is what the camera is parented to, it makes sense to do it like this, we never need to reference it again.

        if (openOnStart)
        {
            Camera.main.fieldOfView = playerCamera.FOV;
            playerActivated = true;
            Interact();
        } // Open terminal on start, this could be for like a tutorial or something in the future
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerActivated) // This is for the case of exiting the terminal, eventually I'll figure out a better way of doing this. Maybe Coroutine?
        {
            if (Vector3.Distance(Camera.main.transform.position, previousPosition.position) > 0.05f)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, previousPosition.position, cameraPosSpeed * Time.deltaTime);
                Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, previousPosition.rotation, cameraRotateSpeed * Time.deltaTime);
            }

            return;
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(player.key))) && playerActivated) ExitMenu(); // Menu Exiting

        if (Vector3.Distance(Camera.main.transform.position, cameraPosition.position) > 0.05f)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraPosition.position, cameraPosSpeed * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, cameraPosition.rotation, cameraRotateSpeed * Time.deltaTime);
        } // Menu transition entrance
    }

    #region Menu System
    public void ExitMenu() // This re-enables all the required parts for the player and camera, letting you keep on movin
    {
        playerActivated = false;

        // Proper positioning
        Camera.main.transform.parent = previousPosition;

        playerMovement.canMove = playerCamera.canLook = playerWeapons.canShoot = true;

        // Turn on the viewmodel
        Camera.main.transform.GetChild(0).gameObject.SetActive(true);

        // Fix cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override string GetDescription()
    {
        return description;
    } // Eventually there will be something specific here, which is why its abstract and not built in.

    public override void Interact() // This disables all the requirements and moves the camera.
    {
        playerActivated = true;

        // Turn off the viewmodel
        Camera.main.transform.GetChild(0).gameObject.SetActive(false);

        // Proper positioning
        Camera.main.transform.parent = cameraPosition;

        // Move the player to the right spot
        playerMovement.rb.velocity = new Vector3(0f, 0f, 0f);
        playerMovement.rb.transform.position = playerPosition.position;

        playerMovement.canMove = playerCamera.canLook = playerWeapons.canShoot = false;

        // Fix cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // FOV Fix
        Camera.main.fieldOfView = 80f;
    }

    public void ChangeScene(int index)
    {
        PlayerPrefs.SetInt("PlayerLevelIndex", index);
        SceneManager.LoadScene(0);
    }

    #endregion
}
