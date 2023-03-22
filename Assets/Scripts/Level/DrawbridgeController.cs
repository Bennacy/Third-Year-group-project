using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawbridgeController : MonoBehaviour
{
    public GameObject blocker;
    public GameObject blocker2;
    public bool blockerActive;
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
        if(blockerActive != blocker.activeSelf){
            blocker.SetActive(blockerActive);
            blocker2.SetActive(blockerActive);
            // GameManager.Instance.FadeOutImage(0.1f, null, Color.black);
            bridgeCam.gameObject.SetActive(blockerActive);
            Time.timeScale = 1;
        }
        
        if(GameManager.Instance.lowerDrawbridge){
            GameManager.Instance.lowerDrawbridge = false;
            // GameManager.Instance.FadeInImage(0.1f, null, Color.black);
            animator.Play("Lower Bridge");
            bridgeCam.gameObject.SetActive(true);
            Time.timeScale = 0;
            // Time.timeScale = Mathf.Abs(Time.timeScale - 1);
        }
        if(GameManager.Instance.raiseDrawbridge){
            GameManager.Instance.raiseDrawbridge = false;
            // GameManager.Instance.FadeInImage(0.1f, null, Color.black);
            animator.Play("Raise Bridge");
            bridgeCam.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
