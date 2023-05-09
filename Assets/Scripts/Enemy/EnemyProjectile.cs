using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public ParticleSystem mainParticle;
    public ParticleSystem trailParticle;
    public ParticleSystem explosionParticle;
    
    void Start()
    {
        // particle.startLifetime = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupProjectile(EnemyAttackScriptableObject attack){
        var mainSettings = mainParticle.main;
        mainSettings.startLifetime = attack.projectileLifetime;
        mainSettings.startSize = transform.lossyScale.x;

        mainParticle.Play(false);
        trailParticle.Play(false);
    }
}
