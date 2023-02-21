using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Collider weaponCollider;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponCollider.enabled){
            Debug.Log("Active!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
    }
}
