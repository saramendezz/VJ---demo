using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float laneDistance = 2; // distance between lanes
    public float jumpForce = 7f;
    public float speed = 6;
    public Rigidbody rb;
    public PlayableDirector timeline;

    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    private bool isGrounded = true;
    private Animator m_Animator;
    private float maxTimeJump = 0f;
    private float offsetTimeJump = 1f;
    private bool isJumping;

    bool alive, startState;

    private void Start()
    {
        startState = true; alive = true; isJumping = false;
        m_Animator = GetComponent<Animator>();
        timeline = GetComponent<PlayableDirector>();
        timeline.Stop();
    }

    void FixedUpdate()
    {
        if (!alive || startState) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 targetPosition = rb.position + forwardMove;
        targetPosition.x = Mathf.Lerp(rb.position.x, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
        rb.MovePosition(targetPosition);
    }

    void Update()
    {
        if (startState && Input.GetKeyDown(KeyCode.P))
        {
            timeline.Play();
        }

        if (startState) return;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
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
            isJumping = true;
            maxTimeJump = Time.time + offsetTimeJump;
        }
        if (isJumping && Time.time > maxTimeJump)
        {
            m_Animator.SetTrigger("stopJumping");
            isJumping = false;
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
    }

    public void startRun()
    {
        startState = false;
        m_Animator.SetTrigger("startRuning");
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
