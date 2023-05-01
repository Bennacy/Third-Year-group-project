using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;
    public AudioMixer audioMixer;
    public float volume;
    public Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public Stack<GameObject> panels;
    public GameObject firstPanel;
    public GameObject settingsPanel;


    // Start is called before the first frame update
    void Start()
    {
        panels = new Stack<GameObject>();
        panels.Push(firstPanel);
        
        SetVolume(volumeSlider.value);
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++){
            Resolution resolution = resolutions[i];
            options.Add($"{resolution.width}x{resolution.height}");

            if(Screen.currentResolution.width == resolution.width && Screen.currentResolution.height == resolution.height){
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float newVolume){
        volume = newVolume;
        audioMixer.SetFloat("MasterVolume", volume);

        volumeText.text = Mathf.Round(volume.Remap(-80, 0, 0, 100)).ToString();
    }

    // public void SetQuality(int qualityIndex){
    //     QualitySettings.SetQualityLevel(qualityIndex);
    // }

    public void SetFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex){
        Screen.SetResolution(Screen.resolutions[resolutionIndex].width, Screen.resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    public void OpenPanel(GameObject panel){
        // Debug.Log(panel);        
        panels.Push(panel);
        panels.Peek().SetActive(true);
    }
    public void OpenPanelClosePrevious(GameObject panel){
        // Debug.Log(panel);
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
    }
}

public static class ExtensionMethods {
 
public static float Remap (this float value, float from1, float to1, float from2, float to2) {
    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
}
   
}