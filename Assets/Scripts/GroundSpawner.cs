using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile001;
    public GameObject groundTile002;
    public GameObject groundTileStart001;
    public GameObject groundTileStart002;
    public GameObject groundTurn;
    public GameObject groundTurnL;
    public GameObject groundTurnR;
    private int currentId, countId, currentFreeLine;
    private const int MAX_NUM_GROUND = 3, MAX_NUM_ITER = 10; 
    Vector3 nextSpawnPoint, nextDoblePoint;

    private Quaternion currentRotation, currentDobleRotation;
    private bool isDoubleDirection;

    private List<GameObject> turnGroundTilesRight, turnGroundTilesLeft;

    public void SpawnTile()
    {
        GameObject tmp, tmp2;
        if (countId == MAX_NUM_ITER)
        {
            int turnType = Random.Range(1, 4);
            switch (turnType)
            {
                case 1:
                    tmp = Instantiate(groundTurn, nextSpawnPoint, currentRotation);
                    break;
                case 2:
                    tmp = Instantiate(groundTurnL, nextSpawnPoint, currentRotation);
                    break;
                case 3:
                    tmp = Instantiate(groundTurnR, nextSpawnPoint, currentRotation);
                    break;
                default:
                    return;
            }
            // tmp = Instantiate(groundTurn, nextSpawnPoint, currentRotation);
            nextDoblePoint = tmp.transform.GetChild(1).transform.position;
        }
        else
        {
            switch (currentId)
            {
                case 1:
                    tmp = Instantiate(groundTile001, nextSpawnPoint, currentRotation);
                    tmp.GetComponent<GroundTile>().SpawnObstacle(currentFreeLine);
                    // spawnObstacle(tmp);
                    break;
                case 2:
                    tmp = Instantiate(groundTile002, nextSpawnPoint, currentRotation);
                    tmp.GetComponent<GroundTile>().SpawnObstacle(currentFreeLine);
                    //spawnObstacle(tmp);
                    break;
                default:
                    return;
            }
            if (isDoubleDirection)
            {
                turnGroundTilesRight.Add(tmp);
                switch (currentId)
                {
                    case 1:
                        tmp2 = Instantiate(groundTile001, nextDoblePoint, currentDobleRotation);
                        tmp2.GetComponent<GroundTile>().SpawnObstacle(currentFreeLine);
                        //spawnObstacle(tmp2);
                        break;
                    case 2:
                        tmp2 = Instantiate(groundTile002, nextDoblePoint, currentDobleRotation);
                        tmp2.GetComponent<GroundTile>().SpawnObstacle(currentFreeLine);
                        //spawnObstacle(tmp2);
                        break;
                    default:
                        return;
                }
                turnGroundTilesLeft.Add(tmp2);
                nextDoblePoint = tmp2.transform.GetChild(0).transform.position;
            }
        }
        nextSpawnPoint = tmp.transform.GetChild(0).transform.position;
        ++countId;
        if (countId > MAX_NUM_ITER) 
        {
            ++currentId;
            if (currentId == MAX_NUM_GROUND) currentId = 1;
            countId = 0;
        }
        int newFreeLine = Random.Range(1, 4);
        while (newFreeLine == currentFreeLine) newFreeLine = Random.Range(1, 4);
        currentFreeLine = newFreeLine;
    }
    void Start()
    {
        currentId = 1; countId = 2; currentFreeLine = 2;
        currentRotation = Quaternion.identity;
        isDoubleDirection = false;
        GameObject tmp;
        tmp = Instantiate(groundTileStart001, nextSpawnPoint, currentRotation);
        nextSpawnPoint = tmp.transform.GetChild(0).transform.position;

        nextDoblePoint = Vector3.zero;

        for (int i = 0; i < 8; i++)
        {
            SpawnTile();
        }
        turnGroundTilesRight = new List<GameObject>();
        turnGroundTilesLeft = new List<GameObject>();
    }

    public void changeDirection(int idTurn)
    {
        Vector3 currentEulerAngles = currentRotation.eulerAngles;
        
        switch (idTurn) 
        {
            case 0:
                isDoubleDirection = true;
                currentEulerAngles.y += 90f;
                break;
            case 1:
                currentEulerAngles.y += -90f;
                break;
            case 2:
                currentEulerAngles.y += 90f;
                break;
            default:
                break;
        }

        currentEulerAngles.y = Mathf.Repeat(currentEulerAngles.y, 360f);
        currentRotation = Quaternion.Euler(currentEulerAngles);
        if (isDoubleDirection)
        {
            currentEulerAngles.y += 180;
            currentEulerAngles.y = Mathf.Repeat(currentEulerAngles.y, 360f);
            currentDobleRotation = Quaternion.Euler(currentEulerAngles);
        }
        // TODO: ACTIVE isDobleDirection, with an other Rotation, add STOP isDobleDirection, TEST
    }

    public void endDoubleDirection(int directionRemoved)
    {
        isDoubleDirection = false;
        if (directionRemoved == 0)
        {
            foreach (GameObject gameObject in turnGroundTilesLeft)
            {
                Destroy(gameObject);
            }

            // Clear the list after destroying all GameObjects
            turnGroundTilesLeft.Clear();
        }
        else
        {
            // for (int i = 0; i < turnGroundTilesLeft.Count; i++) turnGroundTilesLeft.RemoveAt(i);
            foreach (GameObject gameObject in turnGroundTilesRight)
            {
                Destroy(gameObject);
            }

            // Clear the list after destroying all GameObjects
            turnGroundTilesRight.Clear();

            currentRotation = currentDobleRotation;
            nextSpawnPoint = nextDoblePoint;
        }
    }
}
