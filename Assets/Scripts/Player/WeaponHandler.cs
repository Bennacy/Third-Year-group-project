using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Collider weaponCollider;
    public Collider shieldCollider;
    public Transform rightHand;
    public Transform leftHand;
    public GameObject weaponPrefab;


    void Start()
    {
        SwitchWeapon(weaponPrefab);
    }


    void Update()
    {
        
    }

    public void SwitchWeapon(GameObject prefab){
        Instantiate(prefab, rightHand);
    }
}
