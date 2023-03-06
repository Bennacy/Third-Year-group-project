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

    public float attackDelay = 1f;
    

    [SerializeField]
    private Animator animator;
    private WeaponHandler weaponHandler;
    

    

    private const string ATTACK = "Attack";

    public virtual void Start()
    {
        SetupEnemyFromConfig();
        weaponHandler = GetComponent<WeaponHandler>();
    }

    private void Awake()
    {
       
    }


    private void Update()
    {
        Attack();
       
    }

    

    public void Attack()
    {
        if (agent.remainingDistance < 2.5f)
        {

            animator.SetBool(ATTACK, true);
            
        }
        else
        {
            animator.SetBool(ATTACK, false);
            
        }
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
        attackDelay = enemyScriptableObject.attackDelay;


        Debug.Log("Enemy Health is: " + health);

    }



    public void EnableWeaponCollider()
    {
        weaponHandler.ToggleWeaponCollider(true);
    }
    public void DisableWeaponCollider()
    {
        weaponHandler.ToggleWeaponCollider(false);
    }


    public void EndAttack()
    {

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
