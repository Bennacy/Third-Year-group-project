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
    
    public TextMeshProUGUI uiLabel;
    public Slider uiSlider;
    
    void Start()
    {
        SetSliders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetSliders(){
        Debug.Log("Setting sliders");
        masterSlider.value = AudioManager.Instance.masterVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        musicSlider.value = AudioManager.Instance.musicVolume;
        uiSlider.value = AudioManager.Instance.uiVolume;

        SetMasterVolume(AudioManager.Instance.masterVolume);
        SetSFXVolume(AudioManager.Instance.sfxVolume);
        SetMusicVolume(AudioManager.Instance.musicVolume);
        SetUIVolume(AudioManager.Instance.uiVolume);
    }

    public void SetMasterVolume(float value){
        masterLabel.text = Mathf.RoundToInt(value*100).ToString();
        AudioManager.Instance.SetMasterVolume(value);
        AudioManager.Instance.PlayUISlider();
    }

    public void SetSFXVolume(float value){
        sfxLabel.text = Mathf.RoundToInt(value*100).ToString();
        AudioManager.Instance.SetSFXVolume(value);
        AudioManager.Instance.PlayUISlider();
    }

    public void SetMusicVolume(float value){
        musicLabel.text = Mathf.RoundToInt(value*100).ToString();
        AudioManager.Instance.SetMusicVolume(value);
        AudioManager.Instance.PlayUISlider();
    }

    public void SetUIVolume(float value){
        uiLabel.text = Mathf.RoundToInt(value*100).ToString();
        AudioManager.Instance.SetUIVolume(value);
        AudioManager.Instance.PlayUISlider();
    }
}
