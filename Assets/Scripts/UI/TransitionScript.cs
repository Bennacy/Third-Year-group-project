using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    void Update()
    {
        if(GameManager.Instance.loadingScene){
            GameManager.Instance.loadingScene = false;
            animator.SetTrigger("New Scene");
        }
    }
}
