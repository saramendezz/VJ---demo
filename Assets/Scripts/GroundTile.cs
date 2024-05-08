using System.Net.NetworkInformation;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    // Start is called before the first frame update
    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoins();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject obstaclePrefab;

    void SpawnObstacle()
    {
        //Choose random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(1, 4);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        //Spawn the obstacle at the position
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    public GameObject coinPrefab;

    void SpawnCoins()
    {
        int coinsToSpawn = 10;
        float spacing = 1.0f;

        for (int i = 0; i < coinsToSpawn; i++)
        {
            int coinSpawnIndex = Random.Range(1, 4);
            Transform spawnPoint = transform.GetChild(coinSpawnIndex);

            Vector3 coinPosition = spawnPoint.position;
            coinPosition.z += i * spacing;

            if (coinSpawnIndex == 1) coinPosition.x = spawnPoint.position.x + 1;
            else if (coinSpawnIndex == 3) coinPosition.x = spawnPoint.position.x - 1;
            Instantiate(coinPrefab, coinPosition, Quaternion.identity, transform);
        }
    }
}