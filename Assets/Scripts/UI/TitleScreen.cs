using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    private AudioSource audioSource;
    private bool loweringSound = false;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        audioSource.Play();
        
        loweringSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.loadingScene && !loweringSound){
            StartCoroutine(FadeAudio());
        }
    }

    private IEnumerator FadeAudio(){
        loweringSound = true;
        while(audioSource.volume > 0){
            audioSource.volume -= Time.unscaledDeltaTime/2;
            yield return null;
        }
    }

    public void Quit(){
        Application.Quit();
    }

    public void Load(string sceneName){
        GameManager.Instance.LoadScene(sceneName);
    }
}
