using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;
    MainMenu menu;
    private void OnTriggerEnter(Collider other)
    {
        
        //Check if we collide with the player
        if (other.gameObject.name != "Player")
        {
            return;
        }

        //Add to the player's score
        //GameManager.inst.IncrementScore();
        menu.incrementScore();

        //Destroy the coin
        Destroy(gameObject);
    }
    void Start()
    {
        menu = GameObject.FindObjectOfType<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}