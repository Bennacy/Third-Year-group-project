using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public MoveToTarget movement;
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public int health = 250;

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
    }
}
