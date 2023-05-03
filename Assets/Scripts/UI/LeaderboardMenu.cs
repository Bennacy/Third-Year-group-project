using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    private string scoreJSON;


    void Start()
    {
        // group = GetComponentInChildren<CanvasGroup>();
    }

    void OnEnable()
    {
    }

    public void OpenLeaderboard(){
        
        // PersonalScore personalScore = new PersonalScore(100, 10);
        // GameManager.Instance.highScores.InsertScore(personalScore);
        
        scoreJSON = JsonUtility.ToJson(GameManager.Instance.highScores);
        Debug.Log(scoreJSON);
        StartCoroutine(OpenLeaderboardFade());
    }
    private IEnumerator OpenLeaderboardFade(){
        Debug.Log(group);
        group.alpha = 0;
        group.gameObject.SetActive(true);

        while(group.alpha < 1){
            group.alpha += Time.unscaledDeltaTime*7.5f;
            yield return null;
        }   
    }

    public void CloseLeaderboard(){
        StartCoroutine(CloseLeaderboardFade());
    }
    private IEnumerator CloseLeaderboardFade(){
        group.alpha = 1;

        while(group.alpha > 0){
            group.alpha -= Time.unscaledDeltaTime*7.5f;
            yield return null;
        }   

        group.gameObject.SetActive(false);
    }
}