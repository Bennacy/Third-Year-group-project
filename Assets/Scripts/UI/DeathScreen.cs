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
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
        SetTimeText();
        
        GameManager.Instance.FadeOutImage(1, null, Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetTimeText(){
        int totalTime = Mathf.RoundToInt(GameManager.Instance.time);
        int seconds = totalTime % 60;
        int minutes = (totalTime - seconds) / 60;
        timeText.text = "You survived for " + minutes.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0') + " minutes";

        int kills = GameManager.Instance.enemiesKilled;
        switch(kills){
            case 0: 
                killsText.text = "You didn't kill any enemies";
                break;

            case 1:
                killsText.text = "You killed " + kills + " enemy";
                break;

            default:
                killsText.text = "You killed " + kills + " enemies";
                break;
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
