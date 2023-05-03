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
    private CanvasGroup group;

    private bool previousPause;


    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
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
        AudioManager.Instance.PlayUIClick();
        panels.Push(panel);
        panels.Peek().SetActive(true);
    }
    public void OpenPanelClosePrevious(GameObject panel){
        AudioManager.Instance.PlayUIClick();
        panels.Peek().SetActive(false);

        panels.Push(panel);
        panels.Peek().SetActive(true);
    }

    public void ClosePanel(){
        AudioManager.Instance.PlayUIClick();
        if(panels.Count > 1){
            panels.Pop().SetActive(false);
            panels.Peek().SetActive(true);
            ISelectable selectable = panels.Peek().GetComponent<ISelectable>();
            if(selectable != null){
                selectable.SetSelected();
            }
        }else{
            CloseSettings();
        }
    }

    public void OpenSettings(){     
        AudioManager.Instance.PlayUIClick();
        StartCoroutine(OpenSettingsFade());
    }
    private IEnumerator OpenSettingsFade(){
        group.alpha = 0;
        settingsPanel.SetActive(true);

        while(group.alpha < 1){
            group.alpha += Time.unscaledDeltaTime*7.5f;
            yield return null;
        }   
    }

    public void CloseSettings(){
        AudioManager.Instance.PlayUIClick();
        StartCoroutine(CloseSettingsFade());
    }
    private IEnumerator CloseSettingsFade(){
        group.alpha = 1;

        while(group.alpha > 0){
            group.alpha -= Time.unscaledDeltaTime*7.5f;
            yield return null;
        }   


        ISelectable selectable = GetComponentInParent<ISelectable>();
        if(selectable != null){
            selectable.SetSelected();
        }
        settingsPanel.SetActive(false);
    }

    public void CollapseStack(){
        while(panels.Count > 1){
            ClosePanel();
        }

        CloseSettings();
    }

    public void ClearSave(){
        AudioManager.Instance.PlayUIClick();
        GameManager.Instance.ClearSave();
    }
}

public static class ExtensionMethods {
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}