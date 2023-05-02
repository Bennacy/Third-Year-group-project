using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    private AudioSource audioSource;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Debug.Log(audioSource.isPlaying);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit(){
        Application.Quit();
    }

    public void Load(string sceneName){
        GameManager.Instance.LoadScene(sceneName);
    }
}
