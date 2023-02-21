using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Collider weaponCollider;
    public Collider shieldCollider;
    public Transform rightHand;
    public Transform leftHand;
    public WeaponScript weaponPrefab;
    private WeaponScript currentWeapon;


    void Start()
    {
        SwitchWeapon(weaponPrefab.gameObject);
    }


    void Update()
    {
        
    }

    public void SwitchWeapon(GameObject prefab){
        currentWeapon = Instantiate(prefab, rightHand).GetComponent<WeaponScript>();
    }

    public void ToggleWeaponCollider(bool active){
        currentWeapon.weaponCollider.enabled = active;
    }
}
