using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class EscapistMovement : MonoBehaviour
{
    public float laneDistance = 2; // distance between lanes
    public float jumpForce = 7f;
    public float speed = 6;
    public Rigidbody rb;

    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    private Animator m_Animator;

    bool alive, startState;

    private void Start()
    {
        startState = true; alive = true;
        m_Animator = GetComponent<Animator>();
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
        if (startState) return;
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
}
