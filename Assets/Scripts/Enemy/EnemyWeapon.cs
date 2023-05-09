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
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        IHasHealth health = other.GetComponent<IHasHealth>();
        if(health != null){
            health.Damage(parent.enemyScriptableObject.attack.damage);
        }

        if(GetComponentInParent<Enemy>() == null){
            Destroy(gameObject);
        }
    }
}
