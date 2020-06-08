using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] PowerUpPrefabs;
    [SerializeField] private GameObject enemyContainer = null;
    [SerializeField] private GameObject powerUpContainer;
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private float difficultyIncreaseWait = 30f;
    public bool stopSpawning = false;



    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > difficultyIncreaseWait)
        {
            waitTime--;
            difficultyIncreaseWait = difficultyIncreaseWait + Time.time;
        }
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3);
            while (stopSpawning == false)
            {
                Vector3 spawnRange = new Vector3(Random.Range(-8f, 8f), 7f, 0);
                GameObject newEmemy = Instantiate(enemyPrefab, spawnRange, Quaternion.identity);
                newEmemy.transform.parent = enemyContainer.transform;
                yield return new WaitForSeconds(waitTime);
            }
    }

    public IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3);
        while (stopSpawning == false)
            {
                Vector3 spawnRange = new Vector3(Random.Range(-8f, 8f), 7f, 0);
                int randomPowerUp = Random.Range(0, 3);
                GameObject powerUp = Instantiate(PowerUpPrefabs[randomPowerUp], spawnRange, Quaternion.identity);
                powerUp.transform.parent = powerUpContainer.transform;
                yield return new WaitForSeconds(Random.Range(3, 8));
            }
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }
}
