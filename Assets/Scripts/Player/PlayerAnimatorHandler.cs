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
        animator.SetBool("Walking", playerRb.velocity.magnitude > 0.2f);
    }

    public void StartAttack(){
        if(nextInCombo){
            comboIndex++;
            animator.Play(weaponHandler.currentWeapon.attacks[comboIndex]);
        }else{
            animator.Play(weaponHandler.currentWeapon.attacks[0]);
            nextInCombo = true;
        }
        if(comboIndex == weaponHandler.currentWeapon.attacks.Length - 1){
            nextInCombo = false;
        }
        // animator.Play("Attack");
        controller.attacking = true;
    }
    public void EndAttack(){
        controller.attacking = false;
        StartCoroutine(ComboTimer());
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

    public IEnumerator ComboTimer(){
        int currentIndex = comboIndex;
        yield return new WaitForSeconds(.5f);
        if(currentIndex == comboIndex){
            comboIndex = 0;
            nextInCombo = false;
        }
    }
}
