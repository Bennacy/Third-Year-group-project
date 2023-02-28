using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<WaveScript> waves;
    public List<Vector3> spawnPoints;
    private int waveNum;
    private bool waitForNextWave;
    public List<GameObject> currentEnemies;
    public GameObject prefab;
    
    void Start()
    {
        for(int i = 0; i < 5; i++){
            currentEnemies.Add(Instantiate(prefab, spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(AllDead());
    }

    IEnumerator HandleWave(){
        yield return new WaitForSeconds(10);
    }

    IEnumerator WaveTimeOut(){
        yield return new WaitForSeconds(0);
    }

    bool AllDead(){
        foreach(GameObject obj in currentEnemies){
            if(obj.activeSelf)
                return false;
        }

        return true;
    }

    void OnDrawGizmosSelected()
    {
        foreach(Vector3 pos in spawnPoints)
            Gizmos.DrawSphere(pos, .1f);
    }
}
