using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeController : MonoBehaviour
{
    public List<Enemy> enemies;
    public List<Enemy> attackingEnemies;
    public int attackingMax;
    public float threshold;

    public float radiusAroundTarget = 0.5f;
    PlayerController playerController;

    void Start()
    {
       playerController = GameManager.Instance.playerController;
    }

    void Update()
    {
        if(attackingEnemies.Count < attackingMax && attackingEnemies.Count < enemies.Count){
            Enemy adding = ClosestEnemy();
            adding.currentState.Transition(adding.attackingState);
        }
    }

    private Enemy ClosestEnemy(){
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        

        foreach(Enemy enemy in enemies){
            if(enemy.currentState == enemy.circlingState){
                // Debug.Log(playerController);
                float testingDistance = Vector3.Distance(playerController.transform.position, enemy.transform.position);
                if(testingDistance < closestDistance){
                    closestDistance = testingDistance;
                    closestEnemy = enemy; 
                }
            }
        }

        return closestEnemy;
    }

    public void SurroundPlayer()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if(!attackingEnemies.Contains(enemies[i])){
                enemies[i].agent.SetDestination(new Vector3(
                    playerController.transform.position.x + radiusAroundTarget * Mathf.Cos( Mathf.PI * i / enemies.Count),
                    playerController.transform.position.y,
                    playerController.transform.position.z + radiusAroundTarget * Mathf.Sin( Mathf.PI * i / enemies.Count))
                );
            }
        }
    }
}
