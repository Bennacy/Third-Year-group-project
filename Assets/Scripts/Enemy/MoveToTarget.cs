using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    public Transform player;
    public float updateRate = 0.01f;
    private NavMeshAgent agent;

    [SerializeField]
    private float runDist;

    [SerializeField]
    private float stopDist;

    [SerializeField]
    private Animator animator;

    private const string walkParam = "isMoving";
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

    // Update is called once per frame
    void Update()
    {
        CheckRun();
    }

    private IEnumerator FollowPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(updateRate);

        while (enabled)
        {
            agent.SetDestination(player.transform.position - (player.transform.position - transform.position).normalized * 0.5f);

            yield return wait;
        }
    }

    private void CheckRun()
    {
        float dist = Vector3.Distance(agent.transform.position, player.position);

        animator.SetBool(walkParam, agent.velocity.magnitude > 0.01f && agent.velocity.magnitude < 3.6f);

        animator.SetBool(runParam, agent.velocity.magnitude > 3.6f);

        if (dist < runDist)
        {
            agent.speed = 5;
        }
        else
        {
            agent.speed = 3.5f;
        }

        if (dist < stopDist)
        {
            agent.isStopped = true;
        }
        else 
        {
            agent.isStopped = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.name);
    }
}
