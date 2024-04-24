using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile001;
    public GameObject groundTile002;
    private int currentId, countId;
    private const int MAX_NUM_GROUND = 3, MAX_NUM_ITER = 20; 
    Vector3 nextSpawnPoint;

    public void SpawnTile()
    {
        GameObject tmp;
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
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }

    }
}
