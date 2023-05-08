using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Holds the base stats for an enemy. This can be used for the varying enemies within the game
/// and will allow the stats to be reset if they die.
/// </summary>


[CreateAssetMenu(fileName = "Attack Config", menuName = "ScriptableObject/Attack Config")]
public class EnemyAttackScriptableObject : ScriptableObject
{
    public bool isRanged;
    public int damage;
    public float attackRadius;
    public float attackDelay;

    public Projectile projectilePrefab;
    public Vector3 projectileSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask lineOfSightLayers;

    public void SetupEnemy(Enemy enemy){

    }

    public void Attack(Enemy enemy){
        Debug.Log(enemy.gameObject.name);
    }
}
