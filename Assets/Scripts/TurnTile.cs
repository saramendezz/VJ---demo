using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    PlayerMovement playerMovement;
    private bool isInside;

    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInside)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerMovement.turnPlayer(1);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerMovement.turnPlayer(1);
            }
        }
    }
}
