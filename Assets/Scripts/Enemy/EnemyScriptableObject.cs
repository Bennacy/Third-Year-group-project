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
    //Enemy Stats
    public int health = 250;
    public float attackDelay = 1f;
    public float attackRadius = 1.5f;

    //Navmesh Configs
    public float updateRate = 0.01f;

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

}
