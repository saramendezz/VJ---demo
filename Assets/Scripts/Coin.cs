using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;
    public AudioClip coinSound; // Nuevo AudioClip para el sonido de la moneda
    public AnimationCurve curve;

    private Vector3 finalPosition;

    MainMenu menu;
    private void OnTriggerEnter(Collider other)
    {
        
        //Check if we collide with the player
        if (other.gameObject.name != "Player")
        {
            return;
        }

        //Add to the player's score
        menu.incrementScore();

        //Destroy the coin
        Destroy(gameObject);
    }

    void Start()
    {
        menu = GameObject.FindObjectOfType<MainMenu>();
        // finalPosition = gameObject.transform.position;
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
}