using System.Security.Cryptography;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EscapistMovement : MonoBehaviour
{
    public float laneDistance = 2; // Distance between lanes
    public float speed = 6;
    public Rigidbody rb;
    public CapsuleCollider box;
    private float rayRange;

    private readonly float fowardSpeedMult = 0.00035f;

    private float jumpForce;
    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    private Animator m_Animator;

    bool startState;

    private Directions currentDirection;
    private Vector2 defaultXZposition;
    private Quaternion desiredRotation;
    private float curvePtr, curveSpeed;
    private bool isJumping, isJumpAnim, isGrounded, isGoingDown; 
    private float bottomCoordStart;
    private float offsetJumpAnimation;
    private bool isRotating;

    public AnimationCurve curve;

    private void Start()
    {
        box = GetComponent<CapsuleCollider>();
        startState = true;
        m_Animator = GetComponent<Animator>();
        
        desiredRotation = transform.rotation;
        curvePtr = 0f; curveSpeed = 0.01f;

        currentDirection = Directions.FOWARD;
        defaultXZposition = new Vector2(0, 0);
        jumpForce = 5f;
        isJumping = false; isJumping = false; isGoingDown = false; isGrounded = true; isJumpAnim = false;
        isRotating = false;
        rayRange = 5f;
    }

    void FixedUpdate()
    {
        if (startState) return;

        Vector3 targetPosition = rb.position;
        Vector3 forwardMove = speed * Time.fixedDeltaTime * transform.forward;
        targetPosition += forwardMove;
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

        // Handle ducking timer
        if (!isRotating)
        {
            if (DetectObstacleInDirection() && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                m_Animator.SetTrigger("startJumping");
                isJumping = true; isJumpAnim = true;
                isGrounded = false;
                isGoingDown = false;
                bottomCoordStart = box.bounds.center.y - box.bounds.extents.y;
            }
            else if (isJumping)
            {
                offsetJumpAnimation = (box.bounds.center.y - box.bounds.extents.y) - bottomCoordStart;
                //isGrounded = (box.bounds.center.y - box.bounds.extents.y) < bottomCoordStart + 0.1 && (box.bounds.center.y - box.bounds.extents.y) > bottomCoordStart - 0.5;
                if (offsetJumpAnimation > 1.0f) isGoingDown = true;
                if (offsetJumpAnimation < 0.5f && isGoingDown && isJumpAnim)
                {
                    isJumpAnim = false;
                }
                if (offsetJumpAnimation < 0.01f && isGoingDown)
                {
                    isGoingDown = false;
                    isGrounded = true;
                    isJumping = false;
                }
            }
        }
        else RotateSmoothly();
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

    private bool DetectObstacleInDirection()
    {
        Vector3 direction = Vector3.forward;
        Vector3 point = transform.position;
        point.y += 0.5f;
        Ray theRay = new Ray(point, transform.TransformDirection(direction* rayRange));
        //Debug.Log(theRay);
        //Debug.DrawRay(point, transform.TransformDirection(direction * rayRange));

        if (Physics.Raycast(theRay, out RaycastHit hit, rayRange))
        {
            if (hit.collider.tag == "Obstacle")
            {
                //Debug.Log(hit.collider.tag);
                return true;
            }
        }
        return false;
    }

    public int getDesiredLine()
    {
        return desiredLane;
    }
}
