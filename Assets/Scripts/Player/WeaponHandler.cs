using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Collider weaponCollider;
    public Collider shieldCollider;
    public Transform rightHand;
    public Transform leftHand;
    public WeaponScript[] weaponPrefabs;
    public WeaponScript currentWeapon;
    private WeaponCollider weaponColliderScript;
    public PlayerAnimatorHandler animatorHandler;


    void Start()
    {
        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        SwitchWeapon(weaponPrefabs[0]);
    }


    void Update()
    {
        if(weaponPrefabs.Length <= 1)
            return;

        if(Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != weaponPrefabs[0]){
            SwitchWeapon(weaponPrefabs[0]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != weaponPrefabs[1]){
            SwitchWeapon(weaponPrefabs[1]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && currentWeapon != weaponPrefabs[2]){
            SwitchWeapon(weaponPrefabs[2]);
        }
    }

    public void SwitchWeapon(WeaponScript weapon){
        Debug.Log("Switching");
        if(weaponColliderScript != null){
            Destroy(weaponColliderScript.gameObject);
        }
        if(weapon.leftHand){
            weaponColliderScript = Instantiate(weapon.prefab, leftHand).GetComponentInChildren<WeaponCollider>();
        }else{
            weaponColliderScript = Instantiate(weapon.prefab, rightHand).GetComponentInChildren<WeaponCollider>();
        }
        currentWeapon = weapon;

        if(animatorHandler)
            animatorHandler.SwitchLayer(currentWeapon.weaponType);
    }

    public void ToggleWeaponCollider(bool active){
        if(active)
            weaponColliderScript.Enable();
        else
            weaponColliderScript.Disable();
    }
}
