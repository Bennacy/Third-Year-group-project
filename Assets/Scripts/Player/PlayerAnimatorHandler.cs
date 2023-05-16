using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerAnimatorHandler : MonoBehaviour
{
    private WeaponHandler weaponHandler;
    private PlayerController controller;
    private CameraShake cameraShake;
    private Rigidbody playerRb;
    public Animator animator;
    private PlayerAudio playerAudio;

    public bool nextInCombo;
    public int comboIndex;
    
    void Start()
    {
        playerAudio = GetComponentInParent<PlayerAudio>();
        controller = GetComponentInParent<PlayerController>();
        weaponHandler = GetComponentInParent<WeaponHandler>();
        playerRb = GetComponentInParent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraShake = controller.GetComponentInChildren<CameraShake>();
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
    }
    public void DisableWeaponCollider(){
        weaponHandler.ToggleWeaponCollider(false);
    }

    public void PlaySwingClip(){
        controller.audioSource.pitch = Random.Range(0.5f, 1.5f);
        AudioClip[] clips = weaponHandler.currentWeapon.swingClips;
        controller.audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
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

    public void CameraShake(float test){
        // cameraShake.ShakeRotation(1f, 1f, 1f, .2f);
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, .1f);
    }

    public void Footstep(){
        controller.audioSource.pitch = Random.Range(0.7f, 1.3f);
        controller.audioSource.PlayOneShot(playerAudio.footsteps[Random.Range(0, playerAudio.footsteps.Length)]);
    }
}
