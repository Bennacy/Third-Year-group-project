using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private Animator animator;
    
    void Start()
    {
        GameManager.Instance.FadeOutImage(1, null, Color.black);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
        SetTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetTimeText(){
        int totalTime = Mathf.RoundToInt(GameManager.Instance.time);
        int seconds = totalTime % 60;
        int minutes = totalTime - seconds;

        timeText.text = "You took " + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + " minutes";
    }

    public void Quit(){
        Application.Quit();
    }

    public void Load(string sceneName){
        Debug.Log("Loading " + sceneName);
        GameManager.Instance.LoadScene(sceneName);
    }
}
