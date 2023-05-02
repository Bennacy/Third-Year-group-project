using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public const string MIXER_MASTER = "masterVolume";
    public const string MIXER_SFX = "sfxVolume";
    public const string MIXER_MUSIC = "musicVolume";

    public AudioMixer mixer;
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
        // Debug.Log("Audio values");
    }

    void Start()
    {
        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);
    }

    void Update()
    {
        
    }

    public void SetMasterVolume(float value){
        masterVolume = value;
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
    }
    public void SetSFXVolume(float value){
        sfxVolume = value;
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
    }
    public void SetMusicVolume(float value){
        musicVolume = value;
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
    }
}
