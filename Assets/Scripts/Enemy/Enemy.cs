using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

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
    private EnemyWeapon projectile;
    public AudioSource audioSource;
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
    public bool facingPlayer;
    public bool attacking;

    public int maxHealth { get; set; }
    public int health { get; set; }

    public GameObject healthPickUp;

    public ParticleSystem part;
    public VisualEffect spawnFx;

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
        player = GameManager.Instance.playerController;
        weaponHandler = GetComponentInChildren<EnemyWeapon>();
        part = GetComponentInChildren<ParticleSystem>();
        spawnFx = GetComponentInChildren<VisualEffect>();
        // moveToTarget = GetComponent<MoveToTarget>();

        StartCoroutine("SpawnFX");


        InitializeStates();
    }


    private void Update()
    {
        Vector3 playerPosition = player.transform.position;

        if(lookAtPlayer){
            Quaternion lookRotation = Quaternion.LookRotation(playerPosition - transform.position);

            lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10);
        }
        
        playerPosition.y = transform.position.y;
        Vector3 targetDir = playerPosition - transform.position;

        Debug.DrawLine(transform.position, playerPosition);
        float angle = Vector3.Angle(targetDir, transform.forward);
        facingPlayer = (angle < 10.0f);
        
        currentState.Tick();

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

    public IEnumerator SpawnFX()
    {
        spawnFx.Play();
        yield return new WaitForSeconds(1.5f);
        spawnFx.Stop();
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
        part.Play();
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
        if (health <= 0)
        {
            currentState.Transition(dyingState);
            if(projectile != null){
                StartCoroutine(projectile.DestroyProjectile());
            }
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

    public void SpawnProjectile(){
        audioSource.PlayOneShot(enemyScriptableObject.attack.attackClip);
        EnemyAttackScriptableObject attack = enemyScriptableObject.attack;
        projectile = Instantiate(attack.projectilePrefab.gameObject, transform.position + (transform.up) + (transform.forward*attack.projectileSpawnOffset.x), Quaternion.identity).GetComponent<EnemyWeapon>();
        projectile.parent = this;

        projectile.InitializeProjectile(attack);
    }

    public void ThrowProjectile(){
        audioSource.PlayOneShot(enemyScriptableObject.attack.castClip);
        EnemyAttackScriptableObject attack = enemyScriptableObject.attack;
        projectile.ToggleWeaponCollider(true);
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * attack.projectileVelocity;
    }

     
    void OnDrawGizmosSelected(){
        // Gizmos.DrawSphere(transform.position, attackRange);
    }
}
