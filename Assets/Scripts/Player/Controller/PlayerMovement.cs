using System.Collections;
using System.Collections.Generic;
using CUtilities.Player;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] Transform viewPosition;
    internal Rigidbody rb;

    [Header("States")]
    [HideInInspector] float stateDur; // Duration of the state
    [HideInInspector] public PlayerStates // State data
        state,
        prevState;
    internal void ChangeState(PlayerStates s)
    {
        stateDur = 0;
        prevState = state;
        state = s;
    } // State Change function


    [Header("Movement Parameters")]
    [SerializeField] public bool canMove = true;
    [SerializeField] float // Player Speeds
        walkingSpeed,
        runningSpeed;
    [SerializeField] float // Player Acceleration 
        acceleration,
        deceleration;

    [Header("Slope Parameters")]
    [SerializeField] float maxSlopeAngle;

    [Header("View-Tilt Parameters")]
    internal Vector2 viewTilt, currentTiltSpeed;
    const float smoothDampSpeed = 0.1f,
                maxSmoothDampSpeed = 10f;

    [Header("Movement Variables")]
    [HideInInspector] public Vector2 input;
    [HideInInspector] Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);

        float speed = walkingSpeed;
        float speedI = (input != new Vector2(0f, 0f)) ? acceleration : deceleration;

        Vector3 moveDir = (viewPosition.forward * input.y + viewPosition.right * input.x).normalized * speed;

        CameraTilt(running);

        // Velocity
        Vector3 velocity = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocity, ref currentVelocity, speedI * Time.deltaTime);

        Vector3.ClampMagnitude(rb.velocity, speed);

        Debug.DrawRay(rb.transform.position, GetViewDirection() * 3, Color.black);
    }

    public Vector3 GetViewDirection()
    {
        return Camera.main.transform.forward.normalized;
    }

    void CameraTilt(bool running)
    {
        // Tilt Values
        Vector2 xTiltValues = new(3, 1),
                yTiltValues = new(2, 1);

        // Tilting
        Vector2 tiltStrength = -input * (running ? new Vector2(xTiltValues.x, yTiltValues.x) : new Vector2(xTiltValues.y, yTiltValues.y));

        // Smoothdamp? I hardly know her!
        viewTilt = new(
          Mathf.SmoothDamp(viewTilt.x, tiltStrength.x, ref currentTiltSpeed.x, smoothDampSpeed, maxSmoothDampSpeed, Time.deltaTime),
          Mathf.SmoothDamp(viewTilt.y, tiltStrength.y, ref currentTiltSpeed.y, smoothDampSpeed, maxSmoothDampSpeed, Time.deltaTime));

        viewPosition.rotation *= Quaternion.Euler(new Vector3(-viewTilt.y, 0, viewTilt.x));
    }
}
