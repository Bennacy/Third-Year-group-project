using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private PlayerController playerController;

    private PlayerInput playerInput;
    private InputAction switch1Action;
    private InputAction switch2Action;
    private InputAction switch3Action;


    void Start()
    {
        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        playerController = GetComponent<PlayerController>();
        SwitchWeapon(weaponPrefabs[0]);

        playerInput = GetComponent<PlayerInput>();
        
        switch1Action = playerInput.actions["Weapon Slot 1"];
        switch1Action.performed += context => SwitchWeapon(weaponPrefabs[0]);
        switch2Action = playerInput.actions["Weapon Slot 2"];
        switch2Action.performed += context => SwitchWeapon(weaponPrefabs[1]);
        switch3Action = playerInput.actions["Weapon Slot 3"];
        switch3Action.performed += context => SwitchWeapon(weaponPrefabs[2]);
    }


    void Update()
    {
        if(weaponPrefabs.Length <= 1)
            return;

        // if(Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != weaponPrefabs[0]){
        //     SwitchWeapon(weaponPrefabs[0]);
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != weaponPrefabs[1]){
        //     SwitchWeapon(weaponPrefabs[1]);
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha3) && currentWeapon != weaponPrefabs[2]){
        //     SwitchWeapon(weaponPrefabs[2]);
        // }
    }

    public void SwitchWeapon(WeaponScript weapon){

        if(playerController.attacking || playerController.blocking)
            return;
            
        // Debug.Log("Switching");
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
            
        animatorHandler.EndAttack();
    }

    public void ToggleWeaponCollider(bool active){
        if(active)
            weaponColliderScript.Enable();
        else
            weaponColliderScript.Disable();
    }
}
