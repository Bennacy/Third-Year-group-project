using UnityEngine;
using UnityEngine.AI;


public abstract class EnemyState{
    public Enemy enemy;
    protected Transform transform;
    protected NavMeshAgent agent;

    protected EnemyState(Enemy _enemy){
        enemy = _enemy;
        transform = enemy.transform;
        agent = enemy.agent;
    }
    
    public virtual void Init(){
    }

    public virtual void Tick(){
        if(Time.timeScale == 0)
            return;
            
        // Debug.Log(this);
    }

    public virtual void Transition(EnemyState newState){
        enemy.currentState = newState;
        enemy.currentState.Init();
    }
}






public class EnemyAttacking : EnemyState{
    public EnemyAttacking(Enemy _enemy) : base(_enemy){}

    public override void Init()
    {
        base.Init();

        enemy.lookAtPlayer = true;
        enemy.hordeController.attackingEnemies.Add(enemy);
    }

    public override void Tick()
    {
        base.Tick();
        
        if(Vector3.Distance(enemy.player.transform.position, enemy.transform.position) <= enemy.attackRange){
            enemy.animator.SetTrigger("Attack");
        }else{
            agent.SetDestination(enemy.player.transform.position);
        }
        
        agent.isStopped = enemy.attacking;
        enemy.lookAtPlayer = !enemy.attacking;
    }

    public override void Transition(EnemyState newState)
    {
        base.Transition(newState);

        enemy.hordeController.attackingEnemies.Remove(enemy);
    }
}






public class EnemyRetreating : EnemyState{
    public EnemyRetreating(Enemy _enemy) : base(_enemy){}

    public override void Init()
    {
        base.Init();
        
        enemy.lookAtPlayer = true;
        float distance = enemy.circleDistance + Random.Range(-2.5f, 2.5f);
        Vector3 playerDirection = (enemy.player.transform.position - transform.position).normalized;
        agent.SetDestination(enemy.player.transform.position - playerDirection * distance);
    }

    public override void Tick()
    {
        base.Tick();

        if(Vector3.Distance(transform.position, agent.destination) < 3)
            Transition(enemy.circlingState);
    }
}






public class EnemyCircling : EnemyState{
    public EnemyCircling(Enemy _enemy) : base(_enemy){}
    int circleDirection = 1;
    float distance;

    public override void Init()
    {
        base.Init();

        enemy.lookAtPlayer = true;
        circleDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        distance = enemy.circleDistance + Random.Range(-2.5f, 2.5f);
    }

    public override void Tick()
    {
        base.Tick();

        Vector3 target = transform.position + transform.right*circleDirection*enemy.circleSpeed;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.right*circleDirection, out hit, 2)){
            circleDirection *= -1;
            // Debug.Log("Collided with " + hit.collider.gameObject.name + ", reversing direction");
        }

        Vector3 playerDirection = (enemy.player.transform.position - target).normalized;
        agent.SetDestination(enemy.player.transform.position - playerDirection*distance);
    }
}






public class EnemyIdle : EnemyState{
    public EnemyIdle(Enemy _enemy) : base(_enemy){}
}






public class EnemyDying : EnemyState{
    public EnemyDying(Enemy _enemy) : base(_enemy){}

    public override void Init()
    {
        base.Init();
        
        enemy.lookAtPlayer = false;

        enemy.hordeController.attackingEnemies.Remove(enemy);
        enemy.spawner.spawnedEnemies.Remove(enemy);
        enemy.hordeController.enemies.Remove(enemy);

        Collider[] colliders = enemy.GetComponentsInChildren<Collider>();
        foreach(Collider collider in colliders){
            collider.enabled = false;
        }

        GameManager.Instance.enemiesKilled++;
        GameManager.Instance.score += enemy.enemyScriptableObject.scoreGiven;
        GameManager.Instance.kills++;
        GameManager.Instance.currency += enemy.enemyScriptableObject.scoreGiven;
        
        enemy.animator.Play("Death");
        agent.updatePosition = false;
        agent.updateRotation = false;
        enemy.DisableWeaponCollider();
    }

    public override void Tick()
    {
        base.Tick();
    }
}






public class EnemyPatrolling : EnemyState{
    public EnemyPatrolling(Enemy _enemy) : base(_enemy){}

    int currentIndex;

    public override void Init()
    {
        base.Init();

        currentIndex = 0;
        agent.SetDestination(enemy.patrolPoints[currentIndex]);
    }

    public override void Tick()
    {
        base.Tick();

        if(agent.remainingDistance < 1){
            currentIndex++;
            if(currentIndex >= enemy.patrolPoints.Length)
                currentIndex = 0;

            agent.SetDestination(enemy.patrolPoints[currentIndex]);
        }

        if(Vector3.Distance(transform.position, enemy.player.transform.position) <= enemy.chaseDistance){
            Transition(enemy.circlingState);
        }
    }
}

