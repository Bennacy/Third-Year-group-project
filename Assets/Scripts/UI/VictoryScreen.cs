using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI highScoreText;
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
        int minutes = (totalTime - seconds) / 60;
        timeText.text = "You took " + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + " minutes";

        if(GameManager.Instance.newHighScore){
            highScoreText.text = "New high score!";
        }else{
            int hs_totalTime = Mathf.RoundToInt(GameManager.Instance.highScore);
            int hs_seconds = hs_totalTime % 60;
            int hs_minutes = hs_totalTime - hs_seconds;
            highScoreText.text = "High score: " + hs_minutes.ToString().PadLeft(2, '0') + ":" + hs_seconds.ToString().PadLeft(2, '0');
        }
    }

    public void Quit(){
        Application.Quit();
    }

    public void Load(string sceneName){
        Debug.Log("Loading " + sceneName);
        GameManager.Instance.LoadScene(sceneName);
    }
}
