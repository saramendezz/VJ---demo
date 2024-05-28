using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private float turnSpeed;
    public AudioClip coinSound; // Nuevo AudioClip para el sonido de la moneda
    public AnimationCurve curve;

    private Vector3 finalPosition;
    private AudioSource audioSource;
    private bool isHit;

    MainMenu menu;
    PlayerMovement playerMovement;

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
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        // Add an AudioSource compon    ent if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = coinSound;
        isHit = false;
        turnSpeed = Random.Range(25f, 90f);
    }

    public void setPosition(Vector3 pos)
    {
        finalPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
        Vector3 upPos;
        if (isHit)
        {
            Vector3 playHead = playerMovement.transform.position;
            playHead.y += 1.0f;
            upPos = Vector3.Lerp(transform.position, playHead, curve.Evaluate(Time.fixedDeltaTime * 10));
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0f, 0f, 0f), curve.Evaluate(Time.fixedDeltaTime * 5));
        }
        else upPos = Vector3.Lerp(transform.position, finalPosition, curve.Evaluate(Time.fixedDeltaTime * 10));
        transform.SetPositionAndRotation(upPos, transform.rotation);
        //Debug.Log(upPos);
        //Debug.Log(finalPosition);
    }

    private IEnumerator DestroyAfterSound()
    {
        // Play the coin sound
        audioSource.Play();
        isHit = true;

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Destroy the coin
        Destroy(gameObject);
    }
}
