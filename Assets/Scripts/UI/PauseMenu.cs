using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public Animator animator;
    private bool playingAnim;
    public TransitionScript transitionScript;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Pause Slide Out"));
        if(GameManager.Instance.paused == !menu.activeSelf && !playingAnim && !GameManager.Instance.shopOpen){
            ToggleMenu();
        }
    }

    public void ToggleMenu(){
        if(GameManager.Instance.paused){
            menu.SetActive(true);
            animator.Play("Pause Slide In");
        }else{
            playingAnim = true;
            animator.Play("Pause Slide Out");
        }
    }

    public void Unpause(){
        GameManager.Instance.TogglePause();
        // ToggleMenu();
    }

    public void Quit(){
        AudioManager.Instance.PlayUIClick();
        transitionScript.animator.SetTrigger("New Scene");
        GameManager.Instance.Quit();
    }

    public void BackToMain(){
        AudioManager.Instance.PlayUIClick();
        transitionScript.animator.SetTrigger("New Scene");
        GameManager.Instance.LoadScene("Title Screen");
    }

    public void DisableAll(){
        playingAnim = false;
        menu.SetActive(false);
    }
}
