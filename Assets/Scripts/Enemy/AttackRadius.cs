using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class AttackRadius : MonoBehaviour
{

    [Header("Attack Stats")]
    public int damage = 10;
    public float attackDelay = 0.5f;

    [Space(10)]

    [Header("Collisions")]
    
    public List<GameObject> collisions = new List<GameObject>();
    public SphereCollider sphereCollider;

   

    public NavMeshAgent agent;
    public Animator animator;

    [SerializeField]
    private WeaponCollider weaponCollider;


    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

   

    private void OnTriggerEnter(Collider other)
    {
        
    }

  
    private void OnTriggerExit(Collider other)
    {
        
        
    }

}
