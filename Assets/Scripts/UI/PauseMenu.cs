using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public Animator animator;
    private bool playingAnim;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Pause Slide Out"));
        if(GameManager.Instance.paused == !menu.activeSelf && !playingAnim){
            ToggleMenu();
        }
    }

    private void ToggleMenu(){
        if(GameManager.Instance.paused){
            menu.SetActive(true);
            animator.Play("Pause Slide In");
        }else{
            playingAnim = true;
            animator.Play("Pause Slide Out");
        }
    }

    public void DisableAll(){
        playingAnim = false;
        menu.SetActive(false);
    }
}
