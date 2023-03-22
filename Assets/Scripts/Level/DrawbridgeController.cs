using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawbridgeController : MonoBehaviour
{
    public GameObject blocker;
    public GameObject blocker2;
    public bool blockerActive;
    public bool animating;
    public bool finishedAnimation;
    private Animator animator;
    public Camera bridgeCam;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)){
            if(blockerActive)
                GameManager.Instance.lowerDrawbridge = true;
            else
                GameManager.Instance.raiseDrawbridge = true;
        }

        if(blockerActive != blocker.activeSelf){
            blocker.SetActive(blockerActive);
            blocker2.SetActive(blockerActive);
        }

        if(animating && finishedAnimation){
            animating = false;
            GameManager.Instance.FadeInOutImage(1.25f, null, Color.black);
            GameManager.Instance.hideUI = false;
            bridgeCam.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        
        if(GameManager.Instance.lowerDrawbridge){
            LowerDrawbridge();
        }
        if(GameManager.Instance.raiseDrawbridge){
            RaiseDrawbridge();
        }
    }

    void LowerDrawbridge(){
        GameManager.Instance.lowerDrawbridge = false;
        GameManager.Instance.FadeInOutImage(1.25f, null, Color.black);
        animator.Play("Lower Bridge");
        bridgeCam.gameObject.SetActive(true);
        Time.timeScale = 0;
        animating = true;
        GameManager.Instance.hideUI = true;
        finishedAnimation = false;
    }

    void RaiseDrawbridge(){
        GameManager.Instance.raiseDrawbridge = false;
        GameManager.Instance.FadeInOutImage(1.25f, null, Color.black);
        animator.Play("Raise Bridge");
        bridgeCam.gameObject.SetActive(true);
        Time.timeScale = 0;
        animating = true;
        GameManager.Instance.hideUI = true;
        finishedAnimation = false;
    }
}
