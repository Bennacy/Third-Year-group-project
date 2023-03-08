using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    
    [Header("References")]
    
    public EnemyScriptableObject[] enemyTypes;
    public Transform[] spawnPoints;
    public Transform[] closestSpawnPoints;
    [Range(1, 5)]public int spawnPointCount;
    public List<Enemy> spawnedEnemies;
    [Space(10)]


    [Header("Enemy")]
    public int startingEnemiesPerWave;
    public int totalEnemies = 10;
    private int enemiesRemaining;
    private int enemiesPerWave;
    [Space(10)]

    [Header("Waves")]
    public int numWaves;
    public int totalWaves = 10;
    private int currentWave = 1;
    [Space(10)]
    
    [Header("Time")]
    public int waveDelay;
    public float spawnDelay;

    private void Start()
    {
        enemiesPerWave = startingEnemiesPerWave;
        enemiesRemaining = totalEnemies;
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if(currentWave > totalWaves){
            GameManager.Instance.won = true;
        }
    }

   private IEnumerator SpawnWaves()
    {
        while(GameManager.Instance.playerController == null){
            //Does nothing but wait for a frame
            //For some reason the GameManager sometimes loads the playerController before this script needs it
            //But other times it doesn't, and this script breaks because of null reference
            yield return null;
        }
        
        //for (int i = 0; i < numWaves; i++)
        //{
        //    for (int j = 0; j < enemiesPerWave; j++)
        //    {
        //        GameObject prefab = SelectEnemyType();
        //        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        //        Instantiate(prefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        //    }
        //    enemiesPerWave = enemiesPerWave += Random.Range(1, 5);
        //    yield return new WaitForSeconds(spawnDelay);
        //}


        while (currentWave <= totalWaves)
        {


            while (enemiesRemaining > 0)
            {
                FindClosestSpawnPoints();
                GameObject prefab = SelectEnemyType();
                Transform randomSpawnPoint = closestSpawnPoints[Random.Range(0, closestSpawnPoints.Length)];
                Enemy tempEnemy = Instantiate(prefab, randomSpawnPoint.position, randomSpawnPoint.rotation).GetComponent<Enemy>();
                tempEnemy.transform.parent = transform;
                tempEnemy.spawner = this;
                spawnedEnemies.Add(tempEnemy);


                enemiesRemaining--;

                yield return new WaitForSeconds(spawnDelay);
            }

            if (spawnedEnemies.Count == 0 && enemiesRemaining == 0)
            {
                currentWave++;
                totalEnemies = totalEnemies += Random.Range(0, 6);
                enemiesRemaining = totalEnemies;
                Debug.Log(currentWave);
                yield return new WaitForSeconds(waveDelay);
            }

            yield return null;
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

    void FindClosestSpawnPoints(){
        PlayerController playerController = GameManager.Instance.playerController;
        Debug.Log(playerController);

        closestSpawnPoints = new Transform[spawnPointCount];
        float maxDistance = 0;
        Transform toAdd = null;
        
        for(int i = 0; i < spawnPointCount; i++){
            float smallestDistance = Mathf.Infinity;
            foreach(Transform currentTransform in spawnPoints){
                Vector3 pos = currentTransform.position;
                float distance = Vector3.Distance(pos, playerController.transform.position);

                if(distance < smallestDistance && distance > maxDistance){
                    toAdd = currentTransform;
                    smallestDistance = distance;
                }
            }
            maxDistance = Vector3.Distance(toAdd.position, playerController.transform.position);
            closestSpawnPoints[i] = toAdd;
        }
    }
}
