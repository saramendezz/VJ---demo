using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class EscapistMovement : MonoBehaviour
{
    public float laneDistance = 2; // Distance between lanes
    public float jumpForce = 7f;
    public float speed = 6;
    public Rigidbody rb;
    public CapsuleCollider box;

    private readonly float fowardSpeedMult = 0.0003f;

    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    private Animator m_Animator;

    bool startState;
    private bool isDucking = false;
    private float duckTime = 1.0f; // Time to stay ducked
    private float duckTimer = 0;


    private bool isRotating = false;

    private Directions currentDirection;
    private Vector2 defaultXZposition;
    private Quaternion desiredRotation;
    private float curvePtr, curveSpeed;

    public AnimationCurve curve;

    private void Start()
    {
        startState = true;
        m_Animator = GetComponent<Animator>();
        
        desiredRotation = transform.rotation;
        curvePtr = 0f; curveSpeed = 0.01f;

        currentDirection = Directions.FOWARD;
        defaultXZposition = new Vector2(0, 0);
    }

    void FixedUpdate()
    {
        if (startState) return;

        Vector3 targetPosition = rb.position;
        Vector3 forwardMove = speed * Time.fixedDeltaTime * transform.forward;
        targetPosition += forwardMove;
        /*
        if (isXdirection) targetPosition.x = Mathf.Lerp(rb.position.x, defaultXZposition.x + GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
        else targetPosition.z = Mathf.Lerp(rb.position.z, defaultXZposition.y + GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
         */
        switch (currentDirection)
        {
            case Directions.FOWARD:
                targetPosition.x = Mathf.Lerp(rb.position.x, defaultXZposition.x + GetLanePosition(), curve.Evaluate(Time.fixedDeltaTime * 10)); // Smooth transition to the target lane
                break;
            case Directions.BACK:
                targetPosition.x = Mathf.Lerp(rb.position.x, defaultXZposition.x - GetLanePosition(), curve.Evaluate(Time.fixedDeltaTime * 10)); // Smooth transition to the target lane
                break;
            case Directions.RIGHT:
                targetPosition.z = Mathf.Lerp(rb.position.z, defaultXZposition.y - GetLanePosition(), curve.Evaluate(Time.fixedDeltaTime * 10)); // Smooth transition to the target lane
                break;
            case Directions.LEFT:
                targetPosition.z = Mathf.Lerp(rb.position.z, defaultXZposition.y + GetLanePosition(), curve.Evaluate(Time.fixedDeltaTime * 10)); // Smooth transition to the target lane
                break;
            default:
                targetPosition.x = Mathf.Lerp(rb.position.x, defaultXZposition.x + GetLanePosition(), curve.Evaluate(Time.fixedDeltaTime * 10)); // Smooth transition to the target lane
                break;
        }
        rb.MovePosition(targetPosition);

        speed += fowardSpeedMult;
    }

    void Update()
    {
        if (startState) return;

        // Automatically switch lanes to avoid obstacles
        //DetectObstacles();

        // Handle ducking timer
        if (isDucking)
        {
            duckTimer += Time.deltaTime;
            if (duckTimer >= duckTime)
            {
                StopDucking();
            }
        }
        if (isRotating) RotateSmoothly();
    }

    float GetLanePosition()
    {
        // Calculate X position based on desired lane
        return (desiredLane - 1) * laneDistance;
    }

    public void setDesiredLaneEsc(int line)
    {
        desiredLane = line;
    }

    public void turnEscapist(int direction)
    {
        switch (direction)
        {
            case 0:
                currentDirection = Directions.FOWARD;
                break;
            case 1:
                currentDirection = Directions.BACK;
                break;
            case 2:
                currentDirection = Directions.RIGHT;
                break;
            case 3:
                currentDirection = Directions.LEFT;
                break;
            default:
                break;
        }
    }

    public void rotateEscapist(float rotation)
    {
        if (rotation == 90) desiredLane = Mathf.Clamp(desiredLane - 1, 0, 2);
        else desiredLane = Mathf.Clamp(desiredLane + 1, 0, 2);

        // rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
        desiredRotation = rb.rotation * Quaternion.Euler(0, rotation, 0);
        isRotating = true;
    }

    public void setMiddlePosition(Vector3 middlePos)
    {
        defaultXZposition.x = middlePos.x; // set default x
        defaultXZposition.y = middlePos.z; // set default y
    }

    private void RotateSmoothly()
    {
        curvePtr += curveSpeed;

        rb.MoveRotation(Quaternion.Lerp(transform.rotation, desiredRotation, curve.Evaluate(curvePtr)));

        if (Quaternion.Angle(transform.rotation, desiredRotation) < 0.1f)
        {
            isRotating = false;
            curvePtr = 0f;
        }
    }

    public void startRun()
    {
        startState = false;
        m_Animator.SetTrigger("startRuning");
    }

    private void JumpOrDuck()
    {
        // Implement logic to jump or duck if stuck between obstacles
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            m_Animator.SetTrigger("startJumping");
        }
        else
        {
            StartDucking();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, out _, 0.1f);
    }

    private void StartDucking()
    {
        if (isDucking) return;

        Debug.Log("Start Ducking");
        isDucking = true;
        duckTimer = 0;
        m_Animator.SetTrigger("startDucking");
        box.height = 1; // Assuming the normal height is 2
        box.center = new Vector3(box.center.x, 0.5f, box.center.z); // Adjust the collider center
    }

    private void StopDucking()
    {
        Debug.Log("Stop Ducking");
        isDucking = false;
        m_Animator.ResetTrigger("startDucking");
        box.height = 2; // Return to the normal height
        box.center = new Vector3(box.center.x, 1f, box.center.z); // Reset the collider center
    }
}
