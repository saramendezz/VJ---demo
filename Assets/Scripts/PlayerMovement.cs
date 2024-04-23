using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    bool alive = true;
    public float speed = 6;
    public Rigidbody rb;
    private int desiredLane = 1; // 0: left, 1: middle, 2: right
    public float laneDistance = 2; // distance between lanes
    public float jumpForce = 7f;
    private bool isGrounded = true;

    void FixedUpdate()
    {
        if (!alive) return;

        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 targetPosition = rb.position + forwardMove;
        targetPosition.x = Mathf.Lerp(rb.position.x, GetLanePosition(), Time.fixedDeltaTime * 10); // Smooth transition to the target lane
        rb.MovePosition(targetPosition);
    }

    void Update()
    {
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
        alive = false;
        Invoke("Restart", 2); // Delay before restarting
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
