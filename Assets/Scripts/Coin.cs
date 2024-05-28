<<<<<<< HEAD
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> parent of 6d1da8dd (sounds)
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;
<<<<<<< HEAD
    public AudioClip coinSound; // Nuevo AudioClip para el sonido de la moneda

    MainMenu menu;
=======
>>>>>>> parent of 6d1da8dd (sounds)
    private void OnTriggerEnter(Collider other)
    {
        
        //Check if we collide with the player
        if (other.gameObject.name != "Player")
        {
            return;
        }

        //Add to the player's score
<<<<<<< HEAD
        //GameManager.inst.IncrementScore();
        menu.incrementScore();
=======
        GameManager.inst.IncrementScore();
>>>>>>> parent of 6d1da8dd (sounds)

        //Destroy the coin
        Destroy(gameObject);
    }
    void Start()
    {

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