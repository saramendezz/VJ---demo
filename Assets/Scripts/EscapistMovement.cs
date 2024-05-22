using UnityEngine;

public class EscapistMovement : MonoBehaviour
{
    public float laneDistance = 2; // Distance between lanes
    public float jumpForce = 7f;
    public float speed = 6;
    public Rigidbody rb;
    public CapsuleCollider box;

    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    private Animator m_Animator;
    private readonly float fowardSpeedMult = 0.0003f;

    bool alive, startState;
    private bool isDucking = false;
    private float duckTime = 1.0f; // Time to stay ducked
    private float duckTimer = 0;

    private void Start()
    {
        startState = true;
        alive = true;
        m_Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!alive || startState) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 targetPosition = rb.position + forwardMove;
        targetPosition.x = Mathf.Lerp(rb.position.x, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
        rb.MovePosition(targetPosition);

        speed += fowardSpeedMult;
    }

    void Update()
    {
        if (startState) return;

        // Automatically switch lanes to avoid obstacles
        DetectObstacles();

        // Handle ducking timer
        if (isDucking)
        {
            duckTimer += Time.deltaTime;
            if (duckTimer >= duckTime)
            {
                StopDucking();
            }
        }
    }

    float GetLanePosition()
    {
        // Calculate X position based on desired lane
        return (desiredLane - 1) * laneDistance;
    }

    public void startRun()
    {
        startState = false;
        m_Animator.SetTrigger("startRuning");
    }

    void MoveLane(bool goingRight)
    {
        int targetLane = desiredLane + (goingRight ? 1 : -1);

        if (targetLane < 0 || targetLane > 2) return; // If out of bounds, return

        // Check if there's an obstacle in the target lane
        Vector3 targetPosition = transform.position + Vector3.right * (targetLane - desiredLane) * laneDistance;
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                return; // If there's an obstacle, don't move to that lane
            }
        }

        desiredLane = targetLane;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Detect obstacle and change lane if possible
            if (desiredLane == 0)
            {
                desiredLane = 1; // Move to the middle lane if in the left lane
            }
            else if (desiredLane == 2)
            {
                desiredLane = 1; // Move to the middle lane if in the right lane
            }
            else
            {
                desiredLane = 0; // Move to the left lane if in the middle lane
            }
        }
        else if (collision.gameObject.CompareTag("DuckObstacle"))
        {
            StartDucking();
        }
    }

    private void DetectObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                MoveLane(desiredLane == 0 || (desiredLane == 1 && Random.value > 0.5f));
            }
            else if (hit.collider.CompareTag("DuckObstacle"))
            {
                StartDucking();
            }
        }
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
