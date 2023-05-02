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
    public ScrollRect scrollRect;

    private Vector3 currentPos;

    public RectTransform buttonGlow;
    public RectTransform optionsBtn;
    public RectTransform keybindsBtn;

    public int schemeIndex;
    private int prevSchemeIndex;


    void Start()
    {
        sensSlider.value = GameManager.Instance.sensitivity * 100;
        UpdateSensitivity(sensSlider.value);
    }

    void OnEnable()
    {
        OpenOptions();
    }
    
    public void OpenOptions(){
        options.SetActive(true);
        keybinds.SetActive(false);
        buttonGlow.position = optionsBtn.position;
    }

    public void OpenKeybinds(){
        options.SetActive(false);
        keybinds.SetActive(true);
        buttonGlow.position = keybindsBtn.position;
    }

    public void UpdateSensitivity(float sens){
        GameManager.Instance.sensitivity = sens / 100;
        sensText.text = Mathf.RoundToInt(sens).ToString();
    }

    public void UpdateSchemeIndex(int newIndex){
        schemeIndex = newIndex;

        UpdateBindingDisplays();
    }

    private void UpdateBindingDisplays(){
        keyboardBindings.SetActive(false);
        xBoxBindings.SetActive(false);
        psBindings.SetActive(false);

        Vector3 position = Vector3.zero;
        switch(prevSchemeIndex){
            case 0:
                position = keyboardBindings.transform.position;
            break;

            case 1:
                position = xBoxBindings.transform.position;
            break;

            case 2:
                position = psBindings.transform.position;
            break;
        }

        Debug.Log(position + ", " + prevSchemeIndex);

        keyboardBindings.transform.position = position;
        xBoxBindings.transform.position = position;
        psBindings.transform.position = position;


        switch(schemeIndex){
            case 0:
                keyboardBindings.SetActive(true);
                scrollRect.content = (RectTransform)keyboardBindings.transform;
            break;

            case 1:
                xBoxBindings.SetActive(true);
                scrollRect.content = (RectTransform)xBoxBindings.transform;
            break;

            case 2:
                psBindings.SetActive(true);
                scrollRect.content = (RectTransform)psBindings.transform;
            break;
        }

        prevSchemeIndex = schemeIndex;
    }
}
