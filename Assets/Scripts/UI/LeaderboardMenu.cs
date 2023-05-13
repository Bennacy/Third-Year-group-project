using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Button upBtn;
    [SerializeField] private Button downBtn;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private GameObject entryTemplate;
    [SerializeField] private GameObject info;
    [SerializeField] private LeaderboardEntry[] entries;
    [SerializeField] private int displayOffset;
    [SerializeField] private string scoreJSON;

    public void OpenLeaderboard(){    
        CloseInfo();
        foreach(Transform child in entryContainer){
            Destroy(child.gameObject);
        }

        GameManager.Instance.LoadScores();
        
        scoreJSON = JsonUtility.ToJson(GameManager.Instance.highScores);
        // Debug.Log(scoreJSON);

        displayOffset = 0;
        upBtn.interactable = false;
        entries = new LeaderboardEntry[12];
        for(int i = 0; i < 10; i++){
            LeaderboardEntry entry = Instantiate(entryTemplate, entryContainer).GetComponent<LeaderboardEntry>();
            entries[i] = entry;
            entry.place = i+1;
            entry.score = GameManager.Instance.highScores.scores[i].score;
            entry.time = GameManager.Instance.highScores.scores[i].time;
            entry.kills = GameManager.Instance.highScores.scores[i].kills;
            entry.username = GameManager.Instance.highScores.scores[i].name;

            entry.UpdateDisplay();
            entry.gameObject.SetActive(false);
        }
        for(int i = 0; i < 2; i++){
            LeaderboardEntry entry = Instantiate(entryTemplate, entryContainer).GetComponent<LeaderboardEntry>();
            entries[10+i] = entry;
            
            entry.empty = true;

            entry.UpdateDisplay();
            entry.gameObject.SetActive(false);
        }

        RefreshDisplay();

        StartCoroutine(OpenLeaderboardFade());
    }
    private IEnumerator OpenLeaderboardFade(){
        group.alpha = 0;
        group.gameObject.SetActive(true);

        while(group.alpha < 1){
            group.alpha += Time.unscaledDeltaTime*7.5f;
            yield return null;
        }
    }

    public void CloseLeaderboard(){
        AudioManager.Instance.PlayUIClick();
        StartCoroutine(CloseLeaderboardFade());
    }
    private IEnumerator CloseLeaderboardFade(){
        group.alpha = 1;

        while(group.alpha > 0){
            group.alpha -= Time.unscaledDeltaTime*7.5f;
            yield return null;
        }   

        ISelectable selectable = GetComponentInParent<ISelectable>();
        if(selectable != null){
            selectable.SetSelected();
        }
        group.gameObject.SetActive(false);
    }

    private void RefreshDisplay(){
        foreach(LeaderboardEntry entry in entries){
            entry.gameObject.SetActive(false);
        }
        
        for(int i = 0; i  < 4; i++){
            int index = i + displayOffset*4;
            if(index >= entries.Length){
                break;
            }

            entries[index].gameObject.SetActive(true);
        }
    }

    public void ScrollDown(){
        AudioManager.Instance.PlayUIClick();
        displayOffset++;

        if(displayOffset >= 2){
            downBtn.interactable = false;
        }else{
            downBtn.interactable = true;
        }
        upBtn.interactable = true;

        RefreshDisplay();
    }
    public void ScrollUp(){
        AudioManager.Instance.PlayUIClick();
        displayOffset--;

        if(displayOffset <= 0){
            upBtn.interactable = false;
        }else{
            upBtn.interactable = true;
        }
        downBtn.interactable = true;

        RefreshDisplay();
    }

    public void OpenInfo(){
        AudioManager.Instance.PlayUIClick();
        info.SetActive(true);
    }
    public void CloseInfo(){
        AudioManager.Instance.PlayUIClick();
        info.SetActive(false);
    }
}