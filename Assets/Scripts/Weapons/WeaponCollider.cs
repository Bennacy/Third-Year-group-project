using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public Collider weaponCollider;
    public List<GameObject> hitEntities;
    
    void Start()
    {
        weaponCollider = GetComponent<Collider>();
        weaponCollider.enabled = false;
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
        Debug.Log(other.gameObject);

        if(!CheckIfHit(other.gameObject)){
            hitEntities.Add(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
