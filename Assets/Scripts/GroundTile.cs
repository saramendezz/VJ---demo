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
        //SpawnObstacle();
        // SpawnCoins();
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
            SpawnCoins();
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

    public void SpawnObstacle(int iniPosition)
    {
        Transform spawnPoint = transform.GetChild(iniPosition).transform;

        int numObs = Random.Range(2, 4);
        int jumpPosition = 1;

        if (numObs == 3)
        {
            int randomObs = Random.Range(1, 3);
            switch (randomObs)
            {
                case 1:
                    Instantiate(obstaclePrefab01, spawnPoint.position, transform.rotation, transform);
                    break;
                case 2:
                    Instantiate(obstaclePrefab02, spawnPoint.position, transform.rotation, transform);
                    break;
                default:
                    break;
            }
            jumpPosition = iniPosition;
            ++iniPosition;
            if (iniPosition == 4) iniPosition = 1;
            spawnPoint = transform.GetChild(iniPosition).transform;
        }

        for (int i = 0; i < 2; i++)
        {
            int randomObs = Random.Range(1, 6);
            //Spawn the obstacle at the position
            switch (randomObs)
            {
                case 1:
                    Instantiate(obstaclePrefab01, spawnPoint.position, transform.rotation, transform);
                    break;
                case 2:
                    Instantiate(obstaclePrefab02, spawnPoint.position, transform.rotation, transform);
                    break;
                case 3:
                    Instantiate(obstaclePrefab03, spawnPoint.position, transform.rotation, transform);
                    break;
                case 4:
                    Instantiate(obstaclePrefab04, spawnPoint.position, transform.rotation, transform);
                    break;
                case 5:
                    Instantiate(obstaclePrefab05, spawnPoint.position, transform.rotation, transform);
                    break;
                default:
                    break;
            }
            ++iniPosition;
            if (iniPosition == 4) iniPosition = 1;
            spawnPoint = transform.GetChild(iniPosition).transform;
        }
        freeLine = numObs == 2 ? iniPosition : jumpPosition;
    }

    public GameObject coinPrefab;

    public void SpawnCoins()
    {
        int coinsToSpawn = 10;
        float spacing = 1.0f;

        for (int i = 0; i < coinsToSpawn; i++)
        {
            int coinSpawnIndex = 4;//Random.Range(1, 4);
            Transform spawnPoint = transform.GetChild(coinSpawnIndex);

            Vector3 coinPosition = spawnPoint.position;
            coinPosition.x += Random.Range(-2.5f * spacing, 2.5f * spacing);// i * spacing;
            coinPosition.z += Random.Range(-2.5f * spacing, 2.5f * spacing);// i * spacing;
            coinPosition.y = 0.5f;
            Vector3 spawnPosition = spawnPoint.position;
            spawnPosition.y += 3.0f;
            GameObject tmp = Instantiate(coinPrefab, eMovement.transform.position, transform.rotation, transform);
            // coinPosition = new Vector3(i * spacing, i * spacing, 0);

            tmp.GetComponent<Coin>().setPosition(coinPosition);
        }
    }
}