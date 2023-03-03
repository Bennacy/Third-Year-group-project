using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHasHealth
{
    public MoveToTarget movement;
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public int maxHealth { get; set; }
    public int health { get; set; }

    public virtual void Start()
    {
        SetupEnemyFromConfig();
    }

    public virtual void SetupEnemyFromConfig()
    {
        agent.acceleration = enemyScriptableObject.Acceleration;
        agent.angularSpeed = enemyScriptableObject.AngularSpeed;
        agent.areaMask = enemyScriptableObject.AreaMask;
        agent.avoidancePriority = enemyScriptableObject.AvoidancePriority;
        agent.baseOffset = enemyScriptableObject.BaseOffset;
        agent.height = enemyScriptableObject.Height;
        agent.obstacleAvoidanceType = enemyScriptableObject.ObstacleAvoidanceType;
        agent.radius = enemyScriptableObject.Radius;
        agent.speed = enemyScriptableObject.Speed;
        agent.stoppingDistance = enemyScriptableObject.StoppingDistance;
        movement.updateRate = enemyScriptableObject.updateRate;

        health = maxHealth = enemyScriptableObject.health;

        Debug.Log("Enemy Health is: " + health);

    }

    

    public void Damage(int damageVal)
    {
        health -= damageVal;
        Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");
        if (health <= 0)
        {
            Debug.Log("Killed!!!!");
            Destroy(gameObject);
        }
    }

    public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        Debug.Log(health);
    }
}
