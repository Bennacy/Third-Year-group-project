using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public Collider weaponCollider;
    public List<GameObject> hitEntities;
    private WeaponHandler weapon;
    
    void Start()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
        weapon = GetComponentInParent<WeaponHandler>();
    }

    void Update()
    {
        
    }

    public void Enable(){
        hitEntities.Clear();
        weaponCollider.enabled = true;
    }
    public void Disable(){
        weaponCollider.enabled = false;
    }

    private bool CheckIfHit(GameObject newObj){
        foreach(GameObject obj in hitEntities){
            if(newObj == obj){
                return true;
            }
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject);
        IHasHealth damageable = other.GetComponent<IHasHealth>();
        // Debug.Log(weapon.currentWeapon.damage);
        
        if(!CheckIfHit(other.gameObject) && damageable != null){
            hitEntities.Add(other.gameObject);
            damageable.Damage(weapon.currentWeapon.damage);
        }
    }
}
