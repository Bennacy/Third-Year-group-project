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
    public WeaponScript currentWeapon;
    private WeaponCollider weaponColliderScript;


    void Start()
    {
        SwitchWeapon(weaponPrefab);
    }


    void Update()
    {
        
    }

    public void SwitchWeapon(WeaponScript weapon){
        weaponColliderScript = Instantiate(weapon.prefab, rightHand).GetComponentInChildren<WeaponCollider>();
        currentWeapon = weapon;
    }

    public void ToggleWeaponCollider(bool active){
        if(active)
            weaponColliderScript.Enable();
        else
            weaponColliderScript.Disable();
    }
}
