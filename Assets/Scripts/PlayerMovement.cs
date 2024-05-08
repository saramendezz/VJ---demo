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

    private readonly float timeStartSpeed = 1, timeSpeedMult = 0.0005f;
    
    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    private bool isGrounded = true;
    private bool isDucking = false; //ajupir-se
    private Animator m_Animator;
    private float offsetJumpAnimation;
    private bool isJumping, isJumpAnim, isGoingDown;
    private float bottomCoordStart;
    private float currentTimeSpeed;
    private Directions currentDirection;

    bool alive, startState;

    private void Start()
    {
        startState = true; alive = true; isJumping = false; isGoingDown = false; isGrounded = true;  isJumpAnim = false;
        m_Animator = GetComponent<Animator>();
        timeline = GetComponent<PlayableDirector>();
        box = GetComponent<CapsuleCollider>();
        bottomCoordStart = box.bounds.center.y - box.bounds.extents.y;
        timeline.Stop();
        Time.timeScale = timeStartSpeed;
        currentTimeSpeed = timeStartSpeed;
        currentDirection = Directions.FOWARD;
    }

    void FixedUpdate()
    {
        if (!alive || startState) return;

        Vector3 targetPosition = rb.position;
        switch (currentDirection)
        {
            case Directions.FOWARD:
                Vector3 forwardMove = speed * Time.fixedDeltaTime * transform.forward;
                targetPosition += forwardMove;
                targetPosition.x = Mathf.Lerp(rb.position.x, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
                break;
            case Directions.BACK:
                Vector3 backMove = -1 * speed * Time.fixedDeltaTime * transform.forward;
                targetPosition += backMove;
                targetPosition.x = Mathf.Lerp(rb.position.x, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
                break;
            case Directions.RIGHT:
                Vector3 rightMove = speed * Time.fixedDeltaTime * transform.right;
                targetPosition += rightMove;
                targetPosition.z = Mathf.Lerp(rb.position.z, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
                break;
            case Directions.LEFT:
                Vector3 leftMove = -1 * speed * Time.fixedDeltaTime * transform.right;
                targetPosition += leftMove;
                targetPosition.z = Mathf.Lerp(rb.position.z, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
                break;
            default:
                Vector3 defaultMove = speed * Time.fixedDeltaTime * transform.forward;
                targetPosition += defaultMove;
                targetPosition.x = Mathf.Lerp(rb.position.x, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
                break;
        }
        rb.MovePosition(targetPosition);

        Time.timeScale = currentTimeSpeed;
        currentTimeSpeed += timeSpeedMult;
    }

    void Update()
    {
        if (startState && Input.GetKeyDown(KeyCode.P))
        {
            timeline.Play();
        }

        if (startState) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Mathf.Clamp(value, min, max)
            desiredLane = Mathf.Clamp(desiredLane + 1, 0, 2); // Prevents exceeding lane bounds
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane = Mathf.Clamp(desiredLane - 1, 0, 2); // Prevents exceeding lane bounds
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
        //Codi d'ajupir-se
        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
        {
            isDucking = true;
            m_Animator.SetTrigger("startDucking");
            box.height = 1; // Suponiendo que la altura normal es 2
            box.center = new Vector3(box.center.x, 0.5f, box.center.z); // Ajusta el centro del collider
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) && isDucking)
        {
            isDucking = false;
            m_Animator.SetTrigger("stopDucking");
            box.height = 2; // Restaura la altura normal del collider
            box.center = new Vector3(box.center.x, 1, box.center.z); // Restaura el centro del collider
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

    public void Die()
    {
        m_Animator.SetTrigger("setDie");
        alive = false;
        Invoke("Restart", 2); // Delay before restarting
        Time.timeScale = 1;
    }

    public void startRun()
    {
        startState = false;
        m_Animator.SetTrigger("startRuning");
    }

    public void turnPlayer(int direction)
    {
        switch (direction)
        {
            case 0:
                currentDirection = Directions.FOWARD;
                break;
            case 1:
                currentDirection = Directions.RIGHT;
                break;
            case 2:
                currentDirection = Directions.LEFT;
                break;
            default:
                currentDirection = Directions.FOWARD;
                break;
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
