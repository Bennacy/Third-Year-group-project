using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

[System.Serializable]
public class MusicSegments{
    public AudioClip intro;
    public AudioClip loop;
}


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

    public AudioSource musicSource;
    public AudioSource UIsource;
    private GameObject previousSelected;
    private float lastSliderChange;
    private float defaultMusicVolume;

    private EventSystem eventSystem;
    
    [Header("UI Clips")]
    public AudioClip uiClick;
    public AudioClip uiNavigation;
    public AudioClip uiSlider;

    [Header("Music Clips")]
    public MusicSegments titleScreenMusic;
    public MusicSegments inGameMusic;
    public MusicSegments gameOverMusic;

    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
        GetEventSystem();
    }

    void Start()
    {
        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);
        SetUIVolume(uiVolume);
        lastSliderChange = 0;
        defaultMusicVolume = musicSource.volume;
    }

    void Update()
    {
        lastSliderChange += Time.unscaledDeltaTime;
        
        if(eventSystem.currentSelectedGameObject != previousSelected){
            previousSelected = eventSystem.currentSelectedGameObject;
        }
    }

    public IEnumerator PlayMusic(MusicSegments music){
        musicSource.clip = music.intro;
        musicSource.Play();

        if(music.loop == null)  yield return null;
        
        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(music.intro.length));
        musicSource.clip = music.loop;
        musicSource.Play();
    }

    public IEnumerator FadeOutMusic(){
        float timer = 0;
        while(timer < .5f){
            timer += Time.unscaledDeltaTime;
            musicSource.volume -= Time.unscaledDeltaTime*2;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = defaultMusicVolume;
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
        if(lastSliderChange >= .05f){
            lastSliderChange = 0;
            UIsource.PlayOneShot(uiSlider);
        }
    }
}

public static class CoroutineUtil
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}