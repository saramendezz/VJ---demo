using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    PlayerMovement playerMovement;
    private bool isInside;
    private Directions currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        groundSpawner.changeDirection(0);
        currentDirection = Directions.FOWARD;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInside = false;
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
                groundSpawner.endDoubleDirection(0);
                playerMovement.setMiddlePosition(transform.GetChild(0).transform.position);
                switch (currentDirection)
                {
                    case Directions.FOWARD:
                        playerMovement.turnPlayer(2);
                        break;
                    case Directions.BACK:
                        playerMovement.turnPlayer(3);
                        break;
                    case Directions.RIGHT:
                        playerMovement.turnPlayer(1);
                        break;
                    case Directions.LEFT:
                        playerMovement.turnPlayer(0);
                        break;
                    default:
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                groundSpawner.endDoubleDirection(1);
                playerMovement.setMiddlePosition(transform.GetChild(0).transform.position);
                switch (currentDirection)
                {
                    case Directions.FOWARD:
                        playerMovement.turnPlayer(3);
                        break;
                    case Directions.BACK:
                        playerMovement.turnPlayer(2);
                        break;
                    case Directions.RIGHT:
                        playerMovement.turnPlayer(0);
                        break;
                    case Directions.LEFT:
                        playerMovement.turnPlayer(1);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
