using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public Animator animator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.paused == !menu.activeSelf){
            ToggleMenu();
        }
    }

    private void ToggleMenu(){
        menu.SetActive(!menu.activeSelf);
        if(menu.activeSelf){
            animator.Play("Pause Slide");
        }
    }
}
