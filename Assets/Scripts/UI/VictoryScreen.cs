using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
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
        StartCoroutine(AudioManager.Instance.PlayMusic(AudioManager.Instance.gameOverMusic));
    }

    void SetTimeText(){
        int score = GameManager.Instance.score;
        scoreText.text = "You scored " + score + " points";
        
        int totalTime = Mathf.RoundToInt(GameManager.Instance.time);
        int seconds = totalTime % 60;
        int minutes = (totalTime - seconds) / 60;
        timeText.text = "And lasted " + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + " minutes";

        if(totalTime < 60){
            timeText.text = "And lasted " + seconds.ToString().PadLeft(2, '0') + " seconds";
        }

        int placement = GameManager.Instance.leaderboardPlacement + 1;

        switch(placement){
            case 0:
                highScoreText.fontStyle = FontStyles.Normal;
                highScoreText.text = "But didn't reach the leaderboard";
            break;

            case 1:
                highScoreText.fontStyle = FontStyles.Underline;
                highScoreText.text = "You've reached 1st place!";
            break;

            case 2:
                highScoreText.fontStyle = FontStyles.Underline;
                highScoreText.text = "You've reached 2nd place!";
            break;

            case 3:
                highScoreText.fontStyle = FontStyles.Underline;
                highScoreText.text = "You've reached 3rd place!";
            break;

            default:
                highScoreText.fontStyle = FontStyles.Underline;
                highScoreText.text = "You've reached " + placement + "th place!";
            break;
        }
    }

    public void Quit(){
        AudioManager.Instance.PlayUIClick();
        Application.Quit();
    }

    public void Load(string sceneName){
        AudioManager.Instance.PlayUIClick();
        GameManager.Instance.LoadScene(sceneName);
    }

    public void SubmitName(){
        AudioManager.Instance.PlayUIClick();
        string submitting = inputField.text;
        if(submitting.Length < 6){
            GameManager.Instance.savedScore.name = submitting;
            GameManager.Instance.SaveScores(GameManager.Instance.savedScore);

            nameInput.SetActive(false);
            GetComponentInChildren<LeaderboardMenu>().OpenLeaderboard();
        }
        inputField.text = "";
    }
}
