using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile001;
    public GameObject groundTile002;
    public GameObject groundTileStart001;
    public GameObject groundTileStart002;
    public GameObject groundTurn;
    private int currentId, countId;
    private const int MAX_NUM_GROUND = 3, MAX_NUM_ITER = 20; 
    Vector3 nextSpawnPoint;

    public void SpawnTile()
    {
        GameObject tmp;
        if (countId == MAX_NUM_ITER)
            tmp = Instantiate(groundTile001, nextSpawnPoint, Quaternion.identity);
        else
        {
            switch (currentId)
            {
                case 1:
                    tmp = Instantiate(groundTile001, nextSpawnPoint, Quaternion.identity);
                    break;
                case 2:
                    tmp = Instantiate(groundTile002, nextSpawnPoint, Quaternion.identity);
                    break;
                default:
                    tmp = Instantiate(groundTile001, nextSpawnPoint, Quaternion.identity);
                    break;
            }
        }
        nextSpawnPoint = tmp.transform.GetChild(0).transform.position;
        ++countId;
        if (countId == MAX_NUM_ITER) 
        {
            ++currentId;
            if (currentId == MAX_NUM_GROUND) currentId = 1;
            countId = 0;
        }
    }
    void Start()
    {
        currentId = 1; countId = 0;
        GameObject tmp;
        tmp = Instantiate(groundTileStart001, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = tmp.transform.GetChild(0).transform.position;
        tmp = Instantiate(groundTileStart002, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = tmp.transform.GetChild(0).transform.position;

        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }
    }
}
