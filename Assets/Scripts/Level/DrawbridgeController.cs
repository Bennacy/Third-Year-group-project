using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawbridgeController : MonoBehaviour
{
    public GameObject blocker;
    public GameObject blocker2;
    public bool blockerActive;
    private Animator animator;
    
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
        }
        
        if(GameManager.Instance.lowerDrawbridge){
            GameManager.Instance.lowerDrawbridge = false;
            animator.Play("Lower Bridge");
        }
        if(GameManager.Instance.raiseDrawbridge){
            GameManager.Instance.raiseDrawbridge = false;
            animator.Play("Raise Bridge");
        }
    }
}
