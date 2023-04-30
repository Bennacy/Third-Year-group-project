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

    // Start is called before the first frame update
    void Start()
    {
        SetVolume(volumeSlider.value);
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

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}

public static class ExtensionMethods {
 
public static float Remap (this float value, float from1, float to1, float from2, float to2) {
    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
}
   
}