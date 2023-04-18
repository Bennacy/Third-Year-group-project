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
    public PlayerAnimatorHandler animatorHandler;


    void Start()
    {
        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        SwitchWeapon(weaponPrefab);
    }


    void Update()
    {

    }

    public void SwitchWeapon(WeaponScript weapon){
        Debug.Log("Switching");
        weaponColliderScript = Instantiate(weapon.prefab, rightHand).GetComponentInChildren<WeaponCollider>();
        currentWeapon = weapon;

        // if(animatorHandler)
        //     animatorHandler.SwitchLayer(currentWeapon.idle);
    }

    public void ToggleWeaponCollider(bool active){
        if(active)
            weaponColliderScript.Enable();
        else
            weaponColliderScript.Disable();
    }
}
