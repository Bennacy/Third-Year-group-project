using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawbridgeController : MonoBehaviour
{
    public GameObject blocker;
    public GameObject blocker2;
    public bool blockerActive;
    private Animator animator;
    public TransitionScript transitionScript;
    public GameObject bridgeCamera;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(blockerActive != blocker.activeSelf){
            blocker.SetActive(blockerActive);
            blocker2.SetActive(blockerActive);
        }
        
        if(GameManager.Instance.lowerDrawbridge){
            // GameManager.Instance.paused = true;
            GameManager.Instance.hideUI = true;
            transitionScript.animator.SetTrigger("Fade");
            GameManager.Instance.lowerDrawbridge = false;
            animator.Play("Lower Bridge");
            bridgeCamera.SetActive(true);
        }
        if(GameManager.Instance.raiseDrawbridge){
            GameManager.Instance.raiseDrawbridge = false;
            animator.Play("Raise Bridge");
        }
    }

    private void FinishedLower(){
        GameManager.Instance.hideUI = false;
        // transitionScript.animator.SetTrigger("Fade");
        bridgeCamera.SetActive(false);
        // GameManager.Instance.paused = false;
    }
}
