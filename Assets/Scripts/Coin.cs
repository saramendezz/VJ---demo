using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;
    public AudioClip coinSound; // Nuevo AudioClip para el sonido de la moneda
    public AnimationCurve curve;

    private Vector3 finalPosition;
    private AudioSource audioSource;

    MainMenu menu;

    private void OnTriggerEnter(Collider other)
    {
        // Check if we collide with the player
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // Add to the player's score
        menu.incrementScore();

        // Start the coroutine to play the sound and destroy the coin after a delay
        StartCoroutine(DestroyAfterSound());
    }

    void Start()
    {
        menu = GameObject.FindObjectOfType<MainMenu>();
        // Add an AudioSource component if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = coinSound;
    }

    public void setPosition(Vector3 pos)
    {
        finalPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
        Vector3 upPos = Vector3.Lerp(transform.position, finalPosition, curve.Evaluate(Time.fixedDeltaTime * 10));
        transform.SetPositionAndRotation(upPos, transform.rotation);
        //Debug.Log(upPos);
        //Debug.Log(finalPosition);
    }

    private IEnumerator DestroyAfterSound()
    {
        // Play the coin sound
        audioSource.Play();

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Destroy the coin
        Destroy(gameObject);
    }
}
