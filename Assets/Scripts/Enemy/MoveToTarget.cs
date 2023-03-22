using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private NavMeshAgent agent;
    [Space(10)]

    [Header("Movement")]
    [SerializeField]
    private float runDist;
    [SerializeField]
    private float stopDist;
    public float updateRate = 0.01f;
    [SerializeField]
    bool useMovementPrediction;
    [SerializeField]
    [Range(-1f, 1f)]
    private float movementThreshold = 0f;
    [SerializeField]
    [Range(0.25f, 2)]
    private float movementPredictionTime = 1f;
    [Space(10)]


    [Header("Animation")]
    [SerializeField]
    private Animator animator;
    private const string walkParam = "isMoving";
    private const string runParam = "isRunning";

    public HordeController controller;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        controller = FindObjectOfType<HordeController>();
       // this.enabled = true;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            if (!useMovementPrediction)
            {
                //agent.SetDestination(player.transform.position - (player.transform.position - transform.position).normalized * 0.5f);
                controller.SurroundPlayer();
            }
            else
            {
                Vector3 targetPosition = player.transform.position
                    + player.GetComponent<PlayerController>().AverageVelocity * movementPredictionTime;
                Vector3 directionToTarget = (targetPosition - transform.position).normalized;
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

                
                

                float dot = Vector3.Dot(directionToPlayer, directionToTarget);

                if (dot < movementThreshold)
                {
                    targetPosition = player.transform.position;
                }

                agent.SetDestination(targetPosition);
            }


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
