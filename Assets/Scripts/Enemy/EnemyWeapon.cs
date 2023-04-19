using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private Enemy parent;
    private Collider weaponCollider;
    
    void Start()
    {
        parent = GetComponentInParent<Enemy>();
        weaponCollider = GetComponent<Collider>();
    }

    public void ToggleWeaponCollider(bool active){
        weaponCollider.enabled = active;
    }
    
    void OnTriggerEnter(Collider other)
    {
        IHasHealth health = other.GetComponent<IHasHealth>();
        if(health != null){
            health.Damage(50);
        }
    }
}
