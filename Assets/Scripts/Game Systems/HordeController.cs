using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeController : MonoBehaviour
{
    public List<Enemy> enemies;
    public List<Enemy> attackingEnemies;
    public int attackingMaxCount;
    public float attackDistanceThreshold;
    public float teleportDistanceThreshold;

    void Start()
    {
        
    }

    void Update()
    {
        if(attackingEnemies.Count < attackingMaxCount && attackingEnemies.Count < enemies.Count){
            Enemy adding = ClosestEnemy();
            attackingEnemies.Add(adding);
            adding.canAttack = true;
            adding.agent.stoppingDistance = 0.5f;
        }
    }

    private Enemy ClosestEnemy(){
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        PlayerController playerController = GameManager.Instance.playerController;

        foreach(Enemy enemy in enemies){
            if(!enemy.canAttack){
                float testingDistance = Vector3.Distance(playerController.transform.position, enemy.transform.position);
                if(testingDistance < closestDistance){
                    closestDistance = testingDistance;
                    closestEnemy = enemy; 
                }
            }
        }

        return closestEnemy;
    }
}
