using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    private AudioSource audioSource;
    private bool loweringSound = false;
    public TransitionScript transitionScript;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(AudioManager.Instance.PlayMusic(AudioManager.Instance.titleScreenMusic));
    }

    // Update is called once per frame
    void Update()
    {
        // if(GameManager.Instance.loadingScene && !loweringSound){
        //     StartCoroutine(FadeAudio());
        // }
    }

    private IEnumerator FadeAudio(){
        loweringSound = true;
        while(audioSource.volume > 0){
            audioSource.volume -= Time.unscaledDeltaTime/2;
            yield return null;
        }
    }

    public void Quit(){
        transitionScript.animator.SetTrigger("New Scene");
        AudioManager.Instance.PlayUIClick();
        Application.Quit();
    }

    public void Load(string sceneName){
        AudioManager.Instance.PlayUIClick();
        GameManager.Instance.LoadScene(sceneName);
    }
}
