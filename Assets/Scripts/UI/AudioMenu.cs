using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioMenu : MonoBehaviour
{
    public TextMeshProUGUI masterLabel;
    public Slider masterSlider;
    
    public TextMeshProUGUI sfxLabel;
    public Slider sfxSlider;
    
    public TextMeshProUGUI musicLabel;
    public Slider musicSlider;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume(float value){
        masterLabel.text = Mathf.RoundToInt(value*100).ToString();
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void SetSFXVolume(float value){
        sfxLabel.text = Mathf.RoundToInt(value*100).ToString();
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void SetMusicVolume(float value){
        musicLabel.text = Mathf.RoundToInt(value*100).ToString();
        AudioManager.Instance.SetMusicVolume(value);
    }
}
