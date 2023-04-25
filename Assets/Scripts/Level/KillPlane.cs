using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        IHasHealth health;
        if((health = other.GetComponent<IHasHealth>()) != null){
            health.Damage(health.maxHealth);
        }
    }
}
