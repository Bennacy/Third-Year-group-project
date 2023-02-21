using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    [SerializeField]
    private float runDist;

    [SerializeField]
    private float stopDist;

    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        AIMove();
    }

    private void AIMove()
    {
        float dist = Vector3.Distance(agent.transform.position, target.position);

        agent.SetDestination(target.position);

        if(dist < runDist)
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
        Debug.Log(other.name);
    }
}
