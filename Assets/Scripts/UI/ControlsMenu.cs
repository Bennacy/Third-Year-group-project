using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsMenu : MonoBehaviour
{
    public TextMeshProUGUI sensText;
    public Slider sensSlider;
    
    public GameObject options;
    public GameObject keybinds;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void OpenOptions(){
        options.SetActive(true);
        keybinds.SetActive(false);
    }

    public void OpenKeybinds(){
        options.SetActive(false);
        keybinds.SetActive(true);
    }
}
