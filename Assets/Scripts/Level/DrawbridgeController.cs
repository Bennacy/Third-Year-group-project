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
            StartCoroutine(LowerBridge());
        }
        if(GameManager.Instance.raiseDrawbridge){
            GameManager.Instance.raiseDrawbridge = false;
            animator.Play("Raise Bridge");
        }
    }

    private IEnumerator LowerBridge(){
        Time.timeScale = 0;
        transitionScript.animator.SetTrigger("Fade");
        GameManager.Instance.lowerDrawbridge = false;
        GameManager.Instance.hideUI = true;
        
        float wait = 0;
        while(wait < .5f){
            wait += Time.unscaledDeltaTime;
            yield return null;
        }
        bridgeCamera.SetActive(true);
        
        wait = 0;
        while(wait < .7f){
            wait += Time.unscaledDeltaTime;
            yield return null;
        }

        animator.Play("Lower Bridge");
    }

    private void FinishedLower(){
        StartCoroutine(FinisedLowerEnumertaor());
    }
    private IEnumerator FinisedLowerEnumertaor(){
        transitionScript.animator.SetTrigger("Fade");
        
        float wait = 0;
        while(wait < .5f){
            wait += Time.unscaledDeltaTime;
            yield return null;
        }

        bridgeCamera.SetActive(false);

        wait = 0;
        while(wait < .7f){
            wait += Time.unscaledDeltaTime;
            yield return null;
        }

        GameManager.Instance.hideUI = false;
        Time.timeScale = 1;
    }
}
