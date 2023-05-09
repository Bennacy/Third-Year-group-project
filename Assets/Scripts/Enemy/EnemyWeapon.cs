using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Enemy parent;
    private Collider weaponCollider;
    
    void Start()
    {
        if(parent == null)
            parent = GetComponentInParent<Enemy>();

        weaponCollider = GetComponent<Collider>();
    }

    public void ToggleWeaponCollider(bool active){
        weaponCollider.enabled = active;
    }

    public void InitializeProjectile(EnemyAttackScriptableObject attack){
        GetComponent<EnemyProjectile>().SetupProjectile(attack);
        StartCoroutine(Lifetime(attack.projectileLifetime));
    }

    public IEnumerator Lifetime(float time){
        yield return new WaitForSeconds(time);
        StartCoroutine(DestroyProjectile());
    }
    
    void OnTriggerEnter(Collider other)
    {
        IHasHealth health = other.GetComponent<IHasHealth>();
        if(health != null){
            health.Damage(parent.enemyScriptableObject.attack.damage);
        }

        if(GetComponent<EnemyProjectile>() != null){
            StartCoroutine(DestroyProjectile());
        }
    }

    public IEnumerator DestroyProjectile(){
        Debug.Log("Starting Explosion");
        EnemyProjectile projectile = GetComponent<EnemyProjectile>();
        Destroy(GetComponent<Rigidbody>());
        projectile.mainParticle.gameObject.SetActive(false);
        projectile.explosionParticle.Play();
        ToggleWeaponCollider(false);

        yield return new WaitForSeconds(2);
        Debug.Log("Ending Explosion");
        Destroy(gameObject);
    }
}
