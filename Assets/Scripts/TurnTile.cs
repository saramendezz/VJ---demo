using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    PlayerMovement playerMovement;
    CameraFollow cameraFollow;
    private bool isInside;
    private Directions currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
        groundSpawner.changeDirection(0);
        switch (playerMovement.getCurrentDirection())
        {
            case 0:
                currentDirection = Directions.FOWARD;
                break;
            case 1:
                currentDirection = Directions.BACK;
                break;
            case 2:
                currentDirection = Directions.RIGHT;
                break;
            case 3:
                currentDirection = Directions.LEFT;
                break;
            default:
                break;
        }
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
        if (other.gameObject.name == "Player")
        {
            isInside = false;
            groundSpawner.SpawnTile();
            Destroy(gameObject, 2);
        }
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
                playerMovement.rotatePlayer(90f);
                cameraFollow.turnCamera(true, transform.GetChild(0).transform.position);
                switch (currentDirection)
                {
                    case Directions.FOWARD:
                        currentDirection = Directions.RIGHT;
                        playerMovement.turnPlayer(2);
                        break;
                    case Directions.BACK:
                        currentDirection = Directions.LEFT;
                        playerMovement.turnPlayer(3);
                        break;
                    case Directions.RIGHT:
                        currentDirection = Directions.BACK;
                        playerMovement.turnPlayer(1);
                        break;
                    case Directions.LEFT:
                        currentDirection = Directions.FOWARD;
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
                playerMovement.rotatePlayer(-90f);
                cameraFollow.turnCamera(false, transform.GetChild(0).transform.position);
                switch (currentDirection)
                {
                    case Directions.FOWARD:
                        currentDirection = Directions.LEFT;
                        playerMovement.turnPlayer(3);
                        break;
                    case Directions.BACK:
                        currentDirection = Directions.RIGHT;
                        playerMovement.turnPlayer(2);
                        break;
                    case Directions.RIGHT:
                        currentDirection = Directions.FOWARD;
                        playerMovement.turnPlayer(0);
                        break;
                    case Directions.LEFT:
                        currentDirection = Directions.BACK;
                        playerMovement.turnPlayer(1);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
