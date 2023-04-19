using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerController playerController;
    public PlayerInput playerInput;
    public Camera mainCam;

    public Animator uiAnimator;
    public Image globalImage;
    public TextMeshProUGUI globalText;

    public HighScores highScores;

    public bool paused;
    public bool won;
    public bool died;
    public float time;
    public float highScore;
    public int currency;
    public int enemiesKilled;
    public bool newHighScore;
    public bool inGame;
    public bool fading;
    public bool waitForFade;
    public int score;
    public List<Enemy> aliveEnemies;
    public int currentWave;
    public int maxWave;

    [Space(10)]
    [Header("Map Variables")]
    public int drawbridgeState;
    public bool lowerDrawbridge;
    public bool raiseDrawbridge;
    
    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        SceneManager.sceneLoaded += delegate{NewScene();};

        SaveSystem.Init();

        // highScores = new HighScores();
        string scoreString = SaveSystem.Load("High Scores");
        highScores = JsonUtility.FromJson<HighScores>(scoreString);
        // PersonalScore score = new PersonalScore(100, 40, "Bruh");
        // highScores.InsertScore(score, 0);

        // score = new PersonalScore(40, 100, "Bruh2");
        // highScores.InsertScore(score, 0);

        // score = new PersonalScore(999, 100, "Bruh3");
        // highScores.InsertScore(score, 0);

        // string saving = JsonUtility.ToJson(highScores, true);
        // SaveSystem.Save(saving, "High Scores");


        NewScene();
    }

    void Update()
    {
        if(inGame){
            time += Time.deltaTime;
        }
        
        if(waitForFade && !fading){
            waitForFade = false;
            if(won){
                LoadScene("Victory Screen");
                if(highScore > 0 && time < highScore){
                    newHighScore = true;
                    highScore = time;
                }
            }
            if(died){
                LoadScene("Death Screen");
            }
        }

        if(won && inGame && !waitForFade){
            inGame = false;
            FadeInImage(1, null, Color.black);
            waitForFade = true;
        }
        if(died && inGame && !waitForFade){
            
            PersonalScore personalScore = new PersonalScore(score, Mathf.RoundToInt(time));
            highScores.InsertScore(personalScore);
            string saving = JsonUtility.ToJson(highScores, true);
            SaveSystem.Save(saving, "High Scores");

            inGame = false;
            FadeInImage(.3f, null, Color.black);
            waitForFade = true;
        }
    }

    void NewScene(){
        if(GetPlayerController()){
            uiAnimator = GetComponentInChildren<Animator>();
            inGame = true;
            won = false;
            paused = false;
            Time.timeScale = 1;
            died = false;
            newHighScore = false;
            currentWave = 1;
            enemiesKilled = 0;
            time = 0;
            currency = 0;
            score = 0;

            FadeOutImage(1.25f, null, Color.black);
            return;
        }

        inGame = false;
    }

    bool GetPlayerController(){
        Debug.Log("Loaded");
        playerController = FindObjectOfType<PlayerController>();
        if(playerController != null){
            playerInput = playerController.playerInput;
            mainCam = playerController.cameraController.mainCam;
        }

        return playerController != null;
    }

    public void TogglePause(){
        paused = !paused;

        if(paused){
            Time.timeScale = 0;
            playerInput.SwitchCurrentActionMap("UI");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }else{
            Time.timeScale = 1;
            playerInput.SwitchCurrentActionMap("In-Game");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ToggleCursor(){

    }

    public void Quit(){
        Application.Quit();
    }

    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }



    #region Global UI stuff
    public void FadeInImage(float fadeTime, Sprite spriteToUse, Color colorToUse){
        globalImage.sprite = null;
        globalImage.color = colorToUse == null ? Color.black : colorToUse;
        uiAnimator.speed = 1 / fadeTime;

        Debug.Log("Fade In");
        uiAnimator.Play("Fade In");
    }

    public void FadeOutImage(float fadeTime, Sprite spriteToUse, Color colorToUse){
        globalImage.sprite = spriteToUse;
        globalImage.color = colorToUse;
        uiAnimator.speed = 1 / fadeTime;

        Debug.Log("Fade Out");
        uiAnimator.Play("Fade Out");
    }
    #endregion
}
