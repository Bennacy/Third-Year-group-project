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

    public GameObject keyboardBindings;
    public GameObject xBoxBindings;
    public GameObject psBindings;
    public Scrollbar kbScrollBar;
    public Scrollbar xBoxScrollBar;
    public Scrollbar psScrollBar;

    private Vector3 currentPos;

    public int schemeIndex;
    private int prevSchemeIndex;


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

    public void UpdateSchemeIndex(int newIndex){
        schemeIndex = newIndex;

        UpdateBindingDisplays();
    }

    private void UpdateBindingDisplays(){
        keyboardBindings.SetActive(false);
        xBoxBindings.SetActive(false);
        psBindings.SetActive(false);

        float scrollValue = 0;
        switch(prevSchemeIndex){
            case 0:
                scrollValue = kbScrollBar.value;
            break;

            case 1:
                scrollValue = xBoxScrollBar.value;
            break;

            case 2:
                scrollValue = psScrollBar.value;
            break;
        }

        Debug.Log(scrollValue + ", " + prevSchemeIndex);

        kbScrollBar.value = scrollValue;
        xBoxScrollBar.value = scrollValue;
        psScrollBar.value = scrollValue;


        switch(schemeIndex){
            case 0:
                keyboardBindings.SetActive(true);
            break;

            case 1:
                xBoxBindings.SetActive(true);
            break;

            case 2:
                psBindings.SetActive(true);
            break;
        }

        prevSchemeIndex = schemeIndex;
    }
}
