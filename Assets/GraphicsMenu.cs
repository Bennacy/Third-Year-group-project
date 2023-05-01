using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsMenu : MonoBehaviour
{
    public TextMeshProUGUI fovLabel;
    public Slider fovSlider;
    
    public Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    public TMP_Dropdown qualityDropdown;

    public Toggle fullscreenToggle;
    

    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        resolutions = Screen.resolutions;
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

        qualityDropdown.value = QualitySettings.GetQualityLevel();

        fullscreenToggle.isOn = Screen.fullScreen;

        Debug.Log(QualitySettings.GetQualityLevel());

        fovSlider.value = GameManager.Instance.FOV;
        SetFOV(fovSlider.value);
    }

    void Update()
    {
        
    }

    public void SetFOV(float fov){
        fovLabel.text = fov.ToString();

        GameManager.Instance.FOV = Mathf.RoundToInt(fov);
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex){
        Screen.SetResolution(Screen.resolutions[resolutionIndex].width, Screen.resolutions[resolutionIndex].height, Screen.fullScreen);
    }
}
