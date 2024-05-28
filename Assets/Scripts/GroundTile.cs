using System.Net.NetworkInformation;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public GameObject obstaclePrefab01;
    public GameObject obstaclePrefab02;
    public GameObject obstaclePrefab03;
    public GameObject obstaclePrefab04;
    public GameObject obstaclePrefab05;

    GroundSpawner groundSpawner;
    EscapistMovement eMovement;
    PlayerMovement pMovement;

    private int freeLine;
    // Start is called before the first frame update
    void Start()
    {
        eMovement = GameObject.FindObjectOfType<EscapistMovement>();
        pMovement = GameObject.FindObjectOfType<PlayerMovement>();
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoins();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            groundSpawner.SpawnTile();
            Destroy(gameObject, 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Escapist")
        {
            // Debug.Log("Trigger Entered: " + other.gameObject.name);
            eMovement.setDesiredLaneEsc(freeLine-1);
        }
        else if (other.gameObject.name == "Player")
        {
            if (pMovement.getIsGodMode()) pMovement.setDesiredLanePl(freeLine-1);
        }
    }

    public void dieGround()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnObstacle()
    {
        //Choose random point to spawn the obstacle
        int obstacleSpawnIndex = Random.Range(1, 4);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;

        int numObs = Random.Range(2, 4);
        int countArrowPrefab = 0;
        int jumpPosition = -1;
        for (int i = 0; i < numObs; i++)
        {
            int randomObs = Random.Range(1, 6);
            if (countArrowPrefab == 2 && (randomObs == 3 || randomObs == 4)) randomObs = 5;
            //Spawn the obstacle at the position
            switch (randomObs)
            {   
                case 1:
                    Instantiate(obstaclePrefab01, spawnPoint.position, transform.rotation, transform);
                    jumpPosition = obstacleSpawnIndex;
                    break;
                case 2:
                    Instantiate(obstaclePrefab02, spawnPoint.position, transform.rotation, transform);
                    break;
                case 3:
                    Instantiate(obstaclePrefab03, spawnPoint.position, transform.rotation, transform);
                    ++countArrowPrefab; 
                    break;
                case 4:
                    Instantiate(obstaclePrefab04, spawnPoint.position, transform.rotation, transform);
                    ++countArrowPrefab;
                    break;
                case 5:
                    Instantiate(obstaclePrefab05, spawnPoint.position, transform.rotation, transform);
                    break;
                default:
                    Instantiate(obstaclePrefab01, spawnPoint.position, transform.rotation, transform);
                    break;
            }
            ++obstacleSpawnIndex;
            if (obstacleSpawnIndex == 4) obstacleSpawnIndex = 1;
            spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        }
        freeLine = jumpPosition == -1 ? obstacleSpawnIndex : jumpPosition;
        //Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
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

            if (coinSpawnIndex == 1) coinPosition.x = spawnPoint.position.x;
            else if (coinSpawnIndex == 3) coinPosition.x = spawnPoint.position.x;
            Instantiate(coinPrefab, coinPosition, transform.rotation, transform);
        }
    }
}