using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    Vector3 nextSpawnPoint;

    public void SpawnTile()
    {
        GameObject tmp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = tmp.transform.GetChild(1).transform.position;
    }
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }

    }
}
