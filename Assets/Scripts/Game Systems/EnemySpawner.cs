using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int numWaves;
    public float timeBetweenWaves;
    public int startingEnemiesPerWave;
    public EnemyScriptableObject[] enemyTypes;
    public Transform[] spawnPoints;


    private int enemiesPerWave;

    private void Start()
    {
        enemiesPerWave = startingEnemiesPerWave;
        StartCoroutine(SpawnWaves());
    }

   private IEnumerator SpawnWaves()
    {
        for (int i = 0; i < numWaves; i++)
        {
            for (int j = 0; j < enemiesPerWave; j++)
            {
                GameObject prefab = SelectEnemyType();
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(prefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
            }
            enemiesPerWave = enemiesPerWave += Random.Range(1, 5);
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    } 

    /// <summary>
    /// Iterates through the different enemy types, adds up the spawn weight of each enemy type and generates a random number between 0 and the total weight.
    /// Selects the enemy type based on the random number generated and the cumulative weight.
    /// </summary>
    /// <returns> Returns the enemy prefab that has been selected from within the scriptable object</returns>
    GameObject SelectEnemyType()
    {
        float totalWeight = 0;
        foreach (EnemyScriptableObject enemy in enemyTypes)
        {
            totalWeight += enemy.spawnWeight;
        }

        float randomNum = Random.Range(0, totalWeight);
        float weightSum = 0;

        foreach(EnemyScriptableObject enemy in enemyTypes)
        {
            weightSum += enemy.spawnWeight;
            if(randomNum <= weightSum)
            {
                return enemy.prefab;
            }
        }
        return null;
    }
}
