using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

enum Directions
{
    FOWARD, BACK, RIGHT, LEFT
}

public class PlayerMovement : MonoBehaviour
{
    public float laneDistance = 2; // distance between lanes
    public float jumpForce = 7f;
    public float speed = 6;
    public Rigidbody rb;
    public PlayableDirector timeline;
    public CapsuleCollider box;
    public AnimationCurve curve;

    private readonly float timeStartSpeed = 1, timeSpeedMult = 0.0001f, fowardSpeedMult = 0.0003f;

    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    private bool isGrounded = true, isGodMode;
    private Animator m_Animator;
    private float offsetJumpAnimation;
    private bool isJumping, isJumpAnim, isGoingDown, isRotating, isHit;
    private float bottomCoordStart;
    private float currentTimeSpeed, subsSpeed;
    private Directions currentDirection;
    private Vector2 defaultXZposition;
    MainMenu mainMenu;

    //test smooth rotation
    private Quaternion desiredRotation;
    private float curvePtr, curveSpeed;

    bool alive, startState;

    private void Start()
    {
        startState = true; alive = true; isJumping = false; isGoingDown = false; isGrounded = true;  isJumpAnim = false; isRotating = false; isHit = false; isGodMode = false;
        defaultXZposition = new Vector2(0, 0);
        m_Animator = GetComponent<Animator>();
        timeline = GetComponent<PlayableDirector>();
        box = GetComponent<CapsuleCollider>();
        bottomCoordStart = box.bounds.center.y - box.bounds.extents.y;
        timeline.Stop();
        Time.timeScale = timeStartSpeed;
        currentTimeSpeed = timeStartSpeed;
        currentDirection = Directions.FOWARD;
        curvePtr = 0f; curveSpeed = 0.01f;
        desiredRotation = Quaternion.identity;
        subsSpeed = speed;
        mainMenu = GameObject.FindObjectOfType<MainMenu >();
    }

    void FixedUpdate()
    {
        if (!alive || startState) return;

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

        if (Time.timeScale < 3)
        {
            Time.timeScale = currentTimeSpeed;
            currentTimeSpeed += timeSpeedMult;
        }

        speed += fowardSpeedMult;
        subsSpeed += fowardSpeedMult;
    }

    void Update()
    {
        if (startState && Input.GetKeyDown(KeyCode.P))
        {
            mainMenu.startGameFromPlayer();
            timeline.Play();
        }
        bool pActive = false;
        if (isGodMode && Input.GetKeyDown(KeyCode.G))
        {
            mainMenu.exitGodMode();
            isGodMode = false;
            pActive = true;
        }

        if (startState || isGodMode) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Mathf.Clamp(value, min, max)
            if (desiredLane == 2) setIsHit();
            desiredLane = Mathf.Clamp(desiredLane + 1, 0, 2); // Prevents exceeding lane bounds
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (desiredLane == 0) setIsHit();
            desiredLane = Mathf.Clamp(desiredLane - 1, 0, 2); // Prevents exceeding lane bounds
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            m_Animator.SetTrigger("startJumping");
            isJumping = true; isJumpAnim = true;
            isGrounded = false;
            isGoingDown = false;
            bottomCoordStart = box.bounds.center.y - box.bounds.extents.y;
        } else if (isJumping)
        {
            offsetJumpAnimation = (box.bounds.center.y - box.bounds.extents.y) - bottomCoordStart;
            //isGrounded = (box.bounds.center.y - box.bounds.extents.y) < bottomCoordStart + 0.1 && (box.bounds.center.y - box.bounds.extents.y) > bottomCoordStart - 0.5;
            if (offsetJumpAnimation > 1.0f) isGoingDown = true;
            if (offsetJumpAnimation < 0.5f && isGoingDown && isJumpAnim)
            {
                isJumpAnim = false;
                m_Animator.SetTrigger("stopJumping");
            }
            if (offsetJumpAnimation < 0.01f && isGoingDown)
            {
                isGoingDown = false;
                isGrounded = true;
                isJumping = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
        {
            m_Animator.SetTrigger("startDucking");
            box.height = 1; // Suponiendo que la altura normal es 2
            box.center = new Vector3(box.center.x, 0.5f, box.center.z); // Ajusta el centro del collider
        }

        if (isRotating) RotateSmoothly();
        if (isHit) ReturnSpeedSmoothly();

        if (!pActive && !isGodMode && Input.GetKeyDown(KeyCode.G))
        {
            mainMenu.setGodMode();
            isGodMode = true;
        }

        if (transform.position.y < -5)
        {
            Die();
        }   
    }

    float GetLanePosition()
    {
        // Calculate X position based on desired lane
        return (desiredLane - 1) * laneDistance;
    }

    public int getCurrentDirection()
    {
        return ((int)currentDirection);
    }

    public void Die()
    {
        m_Animator.SetTrigger("setDie");
        alive = false;
        Invoke("Restart", 1); // Delay before restarting
        Time.timeScale = 1;
    }

    public void startGame()
    {
        timeline.Play();
    }

    public void startRun()
    {
        startState = false;
        m_Animator.SetTrigger("startRuning");
    }

    public void setMiddlePosition(Vector3 middlePos)
    {
        defaultXZposition.x = middlePos.x; // set default x
        defaultXZposition.y = middlePos.z; // set default y
        //desiredLane = 1;
        //isXdirection = !isXdirection;
    }

    public void turnPlayer(int direction)
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

    public void rotatePlayer(float rotation)
    {
        if (rotation == 90) desiredLane = Mathf.Clamp(desiredLane - 1, 0, 2);
        else desiredLane = Mathf.Clamp(desiredLane + 1, 0, 2);

        // rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
        desiredRotation = rb.rotation * Quaternion.Euler(0, rotation, 0);
        isRotating = true;
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

    private void setIsHit()
    {
        mainMenu.setSlowed();
        if (isHit) Die();
        else
        {
            isHit = true;
            subsSpeed = speed;
            speed -= 3;
        }
    }

    private void ReturnSpeedSmoothly()
    {
        speed = Mathf.Lerp(speed, subsSpeed, curve.Evaluate(Time.fixedDeltaTime * 2));

        if (subsSpeed - speed < 0.1f) 
        {
            mainMenu.exitSlowed();
            isHit = false;
            speed = subsSpeed;
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
