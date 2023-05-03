using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHasHealth
{
    [Header("References")]
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public PlayerController player;
    public Animator animator;
    private EnemyWeapon weaponHandler;
    public EnemySpawner spawner;
    public HordeController hordeController;
    [Space(10)]


    // Enemy states
    public EnemyState currentState;
    public EnemyAttacking attackingState;
    public EnemyCircling circlingState;
    public EnemyRetreating retreatingState;
    public EnemyPatrolling patrollingState;
    public EnemyDying dyingState;
    public EnemyIdle idleState;
    public Vector3[] patrolPoints;
    public float circleDistance;
    public float circleSpeed;
    public float chaseDistance;
    public float attackRange;
    [Space(10)]


    public bool lookAtPlayer;
    public bool attacking;

    public int maxHealth { get; set; }
    public int health { get; set; }

    public GameObject healthPickUp;


    void InitializeStates(){
        attackingState = new EnemyAttacking(this);
        idleState = new EnemyIdle(this);
        circlingState = new EnemyCircling(this);
        retreatingState = new EnemyRetreating(this);
        patrollingState = new EnemyPatrolling(this);
        dyingState = new EnemyDying(this);

        currentState = circlingState;
        currentState.Init();
    }

    public virtual void Start()
    {
        SetupEnemyFromConfig();
        player = GameManager.Instance.playerController;
        weaponHandler = GetComponentInChildren<EnemyWeapon>();
        // moveToTarget = GetComponent<MoveToTarget>();

        InitializeStates();
    }


    private void Update()
    {
        Vector3 playerPosition = player.transform.position;
        currentState.Tick();

        if(lookAtPlayer){
            Quaternion lookRotation = Quaternion.LookRotation(playerPosition - transform.position);

            lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10);
        }

        // if(Vector3.Distance(transform.position, playerPosition) > hordeController.teleportDistanceThreshold){
        //     Transform[] moveTo = spawner.FindClosestSpawnPoints(3);
        //     transform.position = moveTo[Random.Range(0, moveTo.Length)].position;
        // }


        //facingAngle = Vector3.Angle(transform.forward, playerDirection);
        //facingPlayer = (facingAngle < facingThreshold);

        //if(!facingPlayer){
        //    transform.LookAt(playerPosition, Vector3.up);
        //    return;
        //}


        // Attack();
        
        // if(Vector3.Distance(transform.position, playerPosition) > hordeController.attackDistanceThreshold){
        //     animator.SetBool(ATTACK, false);
        //     agent.isStopped = false;
        //     hordeController.attackingEnemies.Remove(this);
        //     agent.stoppingDistance = hordeController.attackDistanceThreshold - 1;
        //     canAttack = false;
        // }
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

        health = maxHealth = enemyScriptableObject.health;

        // Debug.Log("Enemy Health is: " + health);
    }



    public void EnableWeaponCollider()
    {
        weaponHandler.ToggleWeaponCollider(true);
    }
    public void DisableWeaponCollider()
    {
        weaponHandler.ToggleWeaponCollider(false);
    }


    public void BeginAttack()
    {
        attacking = true;
    }

    public void EndAttack()
    {
        attacking = false;
    }

    public void Damage(int damageVal)
    {
        health -= damageVal;
        // StartCoroutine(HitStop());
        // StartCoroutine(player.HitStop());
        // Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");
        if (health <= 0 && animator != null)
        {
            currentState.Transition(dyingState);
        }
        if (animator.enabled == false && health <= 0)
        {
            Die();
        }
    }

    public IEnumerator HitStop(){
        // animator.enabled = false;
        Time.timeScale = 0;
        // agent.updatePosition = false;
        yield return new WaitForSecondsRealtime(.1f);
        Time.timeScale = 1;
        // animator.enabled = true;
        // agent.updatePosition = true;
    }

    public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        Debug.Log(health);
    }

    public void Die(){
        float healthChance = Random.Range(0, 2);
        // Debug.Log("Health: " + healthChance);
        if (healthChance > 0.5)
        {
            Instantiate(healthPickUp, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
        }

        Destroy(gameObject);
    }

     
    void OnDrawGizmosSelected(){
        // Gizmos.DrawSphere(transform.position, attackRange);
    }
}
