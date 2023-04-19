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
            // animator.SetBool("AttackingNext", true);
            animator.Play(weaponHandler.currentWeapon.attacks[comboIndex]);
        }else{
            // animator.SetBool("AttackingNext", true);
            animator.Play(weaponHandler.currentWeapon.attacks[0]);
        }
        nextInCombo = false;
        controller.attacking = true;
        animator.SetBool("Attacking", true);
        DisableWeaponCollider();
    }
    public void EndAttack(){
        controller.attacking = false;
        animator.SetBool("Attacking", false);
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
        animator.Play(weaponHandler.currentWeapon.weaponType + " Block");
        animator.SetBool("Blocking", true);
    }
    public void EndBlock(){
        animator.SetBool("Blocking", false);
    }

    public void EnableWeaponCollider(){
        // animator.SetBool("AttackingNext", false);
        weaponHandler.ToggleWeaponCollider(true);
        controller.audioSource.pitch = Random.Range(0.5f, 1.5f);
        controller.audioSource.PlayOneShot(weaponHandler.currentWeapon.swingClips[0]);
    }
    public void DisableWeaponCollider(){
        weaponHandler.ToggleWeaponCollider(false);
    }

    public void EnableShieldCollider(){

    }
    public void DisableShieldCollider(){

    }
    
    public void SwitchLayer(string animationName){
        animator.Play(animationName + " Idle");
    }

    public void MovementAnimation(){
        animator.SetBool("Sprinting", controller.sprinting);

        animator.SetBool("Walking", controller.walking);
    }

    public void SetTrigger(string triggerName){
        animator.SetTrigger(triggerName);
    }
}
