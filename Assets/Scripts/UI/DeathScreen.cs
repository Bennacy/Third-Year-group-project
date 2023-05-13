using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI killsText;
    private Animator animator;

    public GameObject nameInput;
    public TMP_InputField inputField;
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
        
        if(GameManager.Instance.leaderboardPlacement > -1){
            nameInput.SetActive(true);
        }
        
        SetTimeText();
    }

    void SetTimeText(){
        int score = GameManager.Instance.score;
        killsText.text = "You scored " + score + " points";
        
        int totalTime = Mathf.RoundToInt(GameManager.Instance.time);
        int seconds = totalTime % 60;
        int minutes = (totalTime - seconds) / 60;
        timeText.text = "And lasted " + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + " minutes";

        if(totalTime < 60){
            timeText.text = "And lasted " + seconds.ToString().PadLeft(2, '0') + " seconds";
        }
    }

    public void Quit(){
        AudioManager.Instance.PlayUIClick();
        Application.Quit();
    }

    public void Load(string sceneName){
        AudioManager.Instance.PlayUIClick();
        Debug.Log("Loading " + sceneName);
        GameManager.Instance.LoadScene(sceneName);
    }

    public void SubmitName(){
        AudioManager.Instance.PlayUIClick();
        string submitting = inputField.text;
        if(submitting.Length < 5){
            GameManager.Instance.savedScore.name = submitting;
            GameManager.Instance.SaveScores(GameManager.Instance.savedScore);

            nameInput.SetActive(false);
            GetComponentInChildren<LeaderboardMenu>().OpenLeaderboard();
        }
        inputField.text = "";
    }
}
