using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class MoveToTarget : MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent agent;
    //For later use when we have enemy jump animations if we do
   // private AgentLinkMover linkMover;
    public float updateRate = 0.1f;

    [SerializeField]
    private float runDist;

    [SerializeField]
    private float stopDist;


    [SerializeField]
    private Animator animator;

    private const string moveParam = "isMoving";
    private const string runParam = "isRunning";




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowPlayer());
        
    }

    private void Update()
    {
        RunCheck();
    }

    private IEnumerator FollowPlayer()
    {
        WaitForSeconds Wait = new WaitForSeconds(updateRate);
       
        //Sets enemy to chase player from when they spawn
        while (enabled)
        {
            agent.SetDestination(Player.transform.position - (Player.transform.position - transform.position).normalized * 0.5f);
            yield return Wait;
        }
    }
   
    private void RunCheck()
    {
        //Checks distance between enemy and player to determine if the enemy should be running or walking towards them

        float dist = Vector3.Distance(agent.transform.position, Player.position);

        animator.SetBool(moveParam, agent.velocity.magnitude > 0.01f && agent.velocity.magnitude < 3.6f);

        animator.SetBool(runParam, agent.velocity.magnitude > 3.6f);

        if (dist < runDist)
        {
            agent.speed = 5;
            
        }
        else
        {
            agent.speed = 3.5f;
            
        }

        //Checks if the enemy is too close to the player in which they will stop, so they can move towards attacking

        if (dist < stopDist)
        {
            agent.isStopped = true;
           
        }
        else
        {
            agent.isStopped = false;
        }
    }

}
