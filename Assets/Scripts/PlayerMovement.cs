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

    bool alive, startState;

    private void Start()
    {
        startState = true; alive = true;
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
        if (timeline.state == PlayState.Playing && timeline.time >= timeline.duration)
        {
            startState=false;
            m_Animator.SetTrigger("startRuning");
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

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
