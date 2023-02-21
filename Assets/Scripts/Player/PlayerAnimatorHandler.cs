using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody playerRb;
    private Animator animator;
    
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
        playerRb = GetComponentInParent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("Walking", playerRb.velocity.magnitude > 0.2f);
    }

    public void StartAttack(){
        animator.Play("Attack");
        controller.attacking = true;
    }
    public void EndAttack(){
        controller.attacking = false;
    }

    public void StartBlock(){
        animator.SetBool("Blocking", true);
    }
    public void EndBlock(){
        animator.SetBool("Blocking", false);
    }

    public void EnableWeaponCollider(){
        
    }
    public void DisableWeaponCollider(){

    }

    public void EnableShieldCollider(){

    }
    public void DisableShieldCollider(){
        
    }
}
