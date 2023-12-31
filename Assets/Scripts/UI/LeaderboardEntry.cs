using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    public TextMeshProUGUI placeText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI usernameText;

    public int place;
    public int kills;
    public int score;
    public int time;
    public string username;
    public bool empty = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateDisplay(){
        if(time == -1)
            empty = true;
        
        if(empty){
            placeText.text = "";
            killsText.text = "";
            scoreText.text = "";
            timeText.text = "";
            usernameText.text = "";
        }else{
            int seconds = time % 60;
            int minutes = (time - seconds) / 60;
            

            placeText.text = place.ToString();
            placeText.enabled = true;

            killsText.text = kills.ToString();
            killsText.enabled = true;

            scoreText.text = score.ToString();
            scoreText.enabled = true;

            timeText.text = minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
            timeText.enabled = true;

            usernameText.text = username.ToString();
            usernameText.enabled = true;
        }
    }
}
