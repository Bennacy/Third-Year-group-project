using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Holds the base stats for an enemy. This can be used for the varying enemies within the game
/// and will allow the stats to be reset if they die.
/// </summary>


[CreateAssetMenu(fileName = "Enemy Config", menuName = "ScriptableObject/Enemy Config")]
public class EnemyScriptableObject : ScriptableObject
{
    public GameObject prefab;
    
    //Enemy Stats
    public int health = 250;

    //Navmesh Configs
    public float Acceleration = 8f;
    public float AngularSpeed = 120f;
    public int AreaMask = -1;
    public int AvoidancePriority = 50;
    public float BaseOffset = 0f;
    public float Height = 2f;
    public ObstacleAvoidanceType ObstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    public float Radius = 0.5f;
    public float Speed = 3.5f;
    public float StoppingDistance = 0.5f;
    public int scoreGiven;
    public EnemyAttackScriptableObject attack;

    public float spawnWeight = 1f;


    public void SetUpEnemy(Enemy enemy){
        enemy.agent.acceleration = Acceleration;
        enemy.agent.angularSpeed = AngularSpeed;
        enemy.agent.areaMask = AreaMask;
        enemy.agent.avoidancePriority = AvoidancePriority;
        enemy.agent.baseOffset = BaseOffset;
        enemy.agent.height = Height;
        enemy.agent.obstacleAvoidanceType = ObstacleAvoidanceType;
        enemy.agent.radius = Radius;
        enemy.agent.speed = Speed;
        enemy.agent.stoppingDistance = StoppingDistance;

        enemy.health = enemy.maxHealth = health;
    }

    public void Attack(Enemy enemy){
        Debug.Log(enemy.transform.position);
    }
}
