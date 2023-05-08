using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public ParticleSystem particle;
    
    void Start()
    {
        // particle.startLifetime = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupProjectile(EnemyAttackScriptableObject attack){
        var mainSettings = particle.main;
        mainSettings.startLifetime = attack.projectileLifetime;
        mainSettings.startSize = transform.lossyScale.x;

        particle.Play();
    }
}
