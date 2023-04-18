using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour
{
    private WeaponHandler weaponHandler;
    private PlayerController controller;
    private Rigidbody playerRb;
    private Animator animator;

    public bool nextInCombo;
    public int comboIndex;
    
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
        weaponHandler = GetComponentInParent<WeaponHandler>();
        playerRb = GetComponentInParent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // animator.SetBool("Walking", playerRb.velocity.magnitude > 0.2f);
        MovementAnimation();
    }

    public void StartAttack(){
        if(nextInCombo){
            comboIndex++;
            animator.Play(weaponHandler.currentWeapon.attacks[comboIndex]);
        }else{
            animator.Play(weaponHandler.currentWeapon.attacks[0]);
        }
        nextInCombo = false;
        controller.attacking = true;
        DisableWeaponCollider();
    }
    public void EndAttack(){
        controller.attacking = false;
        DisableComboWindow();
    }

    public void EnableComboWindow(){
        nextInCombo = true;
    }
    public void DisableComboWindow(){
        comboIndex = 0;
        nextInCombo = false;
    }

    public void StartBlock(){
        animator.SetBool("Blocking", true);
    }
    public void EndBlock(){
        animator.SetBool("Blocking", false);
    }

    public void EnableWeaponCollider(){
        weaponHandler.ToggleWeaponCollider(true);
    }
    public void DisableWeaponCollider(){
        weaponHandler.ToggleWeaponCollider(false);
    }

    public void EnableShieldCollider(){

    }
    public void DisableShieldCollider(){

    }
    
    public void SwitchLayer(string animationName){
        animator.Play(animationName);
    }

    public void MovementAnimation(){
        if(controller.sprinting){
            animator.SetBool("Sprinting", true);
            animator.SetBool("Walking", true);
            return;
        }

        if(controller.walking){
            animator.SetBool("Sprinting", false);
            animator.SetBool("Walking", true);
            return;
        }
        
        animator.SetBool("Walking", false);
        animator.SetBool("Sprinting", false);
    }
}
