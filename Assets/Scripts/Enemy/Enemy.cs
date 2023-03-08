using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHasHealth
{
    [Header("References")]
    public MoveToTarget movement;
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public GameObject player;
    [SerializeField]
    private Animator animator;
    private WeaponHandler weaponHandler;
    [Space(10)]

    public float attackDelay = 1f;

    public int maxHealth { get; set; }
    public int health { get; set; }
    public EnemySpawner spawner;

    

    

    

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
        if (health > 0)
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
        damageVal = 50;
        health -= damageVal;
        Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");
        if (health <= 0 && animator != null)
        {
            animator.Play("Death");
            animator.SetBool(ATTACK, false);
            agent.updatePosition = false;
            agent.updateRotation = false;
        }
        if (animator.enabled == false && health <= 0)
        {
            spawner.spawnedEnemies.Remove(this);
            GameManager.Instance.enemiesKilled++;
            Destroy(gameObject);
        }
    }

    public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        Debug.Log(health);
    }

    public void Die(){
        spawner.spawnedEnemies.Remove(this);
        GameManager.Instance.enemiesKilled++;
        Destroy(gameObject);
    }
}
