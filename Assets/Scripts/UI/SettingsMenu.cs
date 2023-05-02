using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Stack<GameObject> panels;
    public GameObject firstPanel;
    public GameObject settingsPanel;

    private bool previousPause;


    // Start is called before the first frame update
    void Start()
    {
        previousPause = false;
        panels = new Stack<GameObject>();
        panels.Push(firstPanel);
    }

    void Update()
    {
        if(previousPause && !GameManager.Instance.paused){
            CollapseStack();
        }
        
        previousPause = GameManager.Instance.paused;
    }

    public void OpenPanel(GameObject panel){
        panels.Push(panel);
        panels.Peek().SetActive(true);
    }
    public void OpenPanelClosePrevious(GameObject panel){
        panels.Peek().SetActive(false);

        panels.Push(panel);
        panels.Peek().SetActive(true);
    }

    public void ClosePanel(){
        if(panels.Count > 1){
            panels.Pop().SetActive(false);
            panels.Peek().SetActive(true);
        }else{
            CloseSettings();
        }
    }

    public void OpenSettings(){
        settingsPanel.SetActive(true);
    }

    public void CloseSettings(){
        settingsPanel.SetActive(false);
    }

    public void CollapseStack(){
        while(panels.Count > 1){
            ClosePanel();
        }

        CloseSettings();
    }
}

public static class ExtensionMethods {
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}