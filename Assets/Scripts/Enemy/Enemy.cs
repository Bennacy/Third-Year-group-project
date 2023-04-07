using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHasHealth
{
    [Header("References")]
    public MoveToTarget movement;
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public PlayerController player;
    [SerializeField] private Animator animator;
    private WeaponHandler weaponHandler;
    private MoveToTarget moveToTarget;
    public EnemySpawner spawner;
    public HordeController hordeController;
    [Space(10)]


    public float attackDelay = 1f;
    public bool canAttack;
    public bool attacking;
    public float facingThreshold;
    public float facingAngle;
    public bool facingPlayer;
    public float rotateSpeed;

    public int maxHealth { get; set; }
    public int health { get; set; }

    public GameObject healthPickUp;


    
    

    

    private const string ATTACK = "Attack";

    public virtual void Start()
    {
        SetupEnemyFromConfig();
        player = GameManager.Instance.playerController;
        weaponHandler = GetComponent<WeaponHandler>();
        moveToTarget = GetComponent<MoveToTarget>();
    }

    private void Awake()
    {
       
    }


    private void Update()
    {
        Vector3 playerPosition = player.transform.position;
        playerPosition.y = transform.position.y;
        Vector3 playerDirection = (playerPosition - transform.position);

        if(Vector3.Distance(transform.position, playerPosition) > hordeController.teleportDistanceThreshold){
            Transform[] moveTo = spawner.FindClosestSpawnPoints(3);
            transform.position = moveTo[Random.Range(0, moveTo.Length)].position;
        }

        facingAngle = Vector3.Angle(transform.forward, playerDirection);
        facingPlayer = (facingAngle < facingThreshold);

        if(!facingPlayer){
            transform.LookAt(playerPosition, Vector3.up);
            return;
        }

        if(!canAttack)
            return;

        Attack();
        
        if(Vector3.Distance(transform.position, playerPosition) > hordeController.attackDistanceThreshold){
            animator.SetBool(ATTACK, false);
            agent.isStopped = false;
            hordeController.attackingEnemies.Remove(this);
            agent.stoppingDistance = hordeController.attackDistanceThreshold - 1;
            canAttack = false;
        }
    }

    

    public void Attack()
    {
        if (health > 0 && !attacking)
        {
            if (agent.remainingDistance < 2.5f)
            {
                animator.SetBool(ATTACK, true);
                agent.isStopped = true;
            }
            else
            {
                animator.SetBool(ATTACK, false);
                agent.isStopped = false;
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
            DisableWeaponCollider();
        }
        if (animator.enabled == false && health <= 0)
        {
            Die();
        }
    }

    public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        Debug.Log(health);
    }

    public void Die(){
        GameManager.Instance.playerController.GetComponent<IHasHealth>().Damage(-10);
        spawner.spawnedEnemies.Remove(this);
        hordeController.enemies.Remove(this);
        if(canAttack)
            hordeController.attackingEnemies.Remove(this);
            
        GameManager.Instance.enemiesKilled++;
        float healthChance = Random.Range(0, 2);
        Debug.Log("Health: " + healthChance);
        if (healthChance > 0.5)
        {
            Instantiate(healthPickUp, transform.position, transform.rotation);
        }


        
        Destroy(gameObject);
        
        
    }

     
    
}
