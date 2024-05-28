using UnityEngine;

public class TurnTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    PlayerMovement playerMovement;
    EscapistMovement escapistMovement;
    CameraFollow cameraFollow;
    private bool isInside, nextRight, userMoveRight;
    private Directions currentDirection;
    private Directions nextDirection;

    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
        escapistMovement = GameObject.FindObjectOfType<EscapistMovement>();
        cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
        groundSpawner.changeDirection(0);
        nextRight = Random.Range(0, 2) == 0 ? true : false;
        userMoveRight = nextRight;
        switch (playerMovement.getCurrentDirection())
        {
            case 0:
                currentDirection = Directions.FOWARD;
                nextDirection = nextRight ? Directions.RIGHT : Directions.LEFT;
                break;
            case 1:
                currentDirection = Directions.BACK;
                nextDirection = nextRight ? Directions.LEFT : Directions.RIGHT;
                break;
            case 2:
                currentDirection = Directions.RIGHT;
                nextDirection = nextRight ? Directions.BACK : Directions.FOWARD;
                break;
            case 3:
                currentDirection = Directions.LEFT;
                nextDirection = nextRight ? Directions.FOWARD : Directions.BACK;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (playerMovement.getIsGodMode())
            {
                playerMovement.setMiddlePosition(transform.GetChild(0).transform.position);
                playerMovement.rotatePlayer(nextRight ? 90f : -90f);
                playerMovement.setDesiredLanePl(1);
                groundSpawner.endDoubleDirection(nextRight ? 0 : 1);
                cameraFollow.turnCamera(nextRight, transform.GetChild(0).transform.position);
                switch (nextDirection)
                {
                    case Directions.FOWARD:
                        playerMovement.turnPlayer(0);
                        break;
                    case Directions.BACK:
                        playerMovement.turnPlayer(1);
                        break;
                    case Directions.RIGHT:
                        playerMovement.turnPlayer(2);
                        break;
                    case Directions.LEFT:
                        playerMovement.turnPlayer(3);
                        break;
                    default:
                        break;
                }
                currentDirection = nextDirection;
            }
            else
            {
                isInside = true;
                playerMovement.setInsideTurn(true);
            }
        }
        else if (other.gameObject.name == "Escapist")
        {
            escapistMovement.setMiddlePosition(transform.GetChild(0).transform.position);
            escapistMovement.rotateEscapist(nextRight ? 90f : -90f);
            escapistMovement.setDesiredLaneEsc(1);
            switch (nextDirection)
            {
                case Directions.FOWARD:
                    escapistMovement.turnEscapist(0);
                    break;
                case Directions.BACK:
                    escapistMovement.turnEscapist(1);
                    break;
                case Directions.RIGHT:
                    escapistMovement.turnEscapist(2);
                    break;
                case Directions.LEFT:
                    escapistMovement.turnEscapist(3);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isInside = false;
            playerMovement.setInsideTurn(false);
            if (userMoveRight != nextRight) playerMovement.Die();
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
                userMoveRight = true;
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
                isInside = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                userMoveRight = false;
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
                isInside = false;
            }
        }
    }
}
