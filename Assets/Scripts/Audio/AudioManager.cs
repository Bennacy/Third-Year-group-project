using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public const string MIXER_MASTER = "masterVolume";
    public const string MIXER_SFX = "sfxVolume";
    public const string MIXER_MUSIC = "musicVolume";
    public const string MIXER_UI = "uiVolume";

    public AudioMixer mixer;
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    public float uiVolume;

    private EventSystem eventSystem;
    private AudioSource UIsource;
    public AudioClip uiClick;
    public AudioClip uiNavigation;
    public AudioClip uiSlider;
    private GameObject previousSelected;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
        UIsource = GetComponent<AudioSource>();
        GetEventSystem();
        // Debug.Log("Audio values");
    }

    void Start()
    {
        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);
        SetUIVolume(uiVolume);
    }

    void Update()
    {
        if(eventSystem.currentSelectedGameObject != previousSelected){
            Debug.Log("Navigation");
            previousSelected = eventSystem.currentSelectedGameObject;
            PlayUINavigation();
        }
    }

    public void GetEventSystem(){
        eventSystem = EventSystem.current;
        previousSelected = eventSystem.currentSelectedGameObject;
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
    public void SetUIVolume(float value){
        uiVolume = value;
        mixer.SetFloat(MIXER_UI, Mathf.Log10(uiVolume) * 20);
    }

    public void PlayUIClick(){
        UIsource.PlayOneShot(uiClick);
    }
    public void PlayUINavigation(){
        UIsource.PlayOneShot(uiNavigation);
    }
    public void PlayUISlider(){
        UIsource.PlayOneShot(uiSlider);
    }
}
