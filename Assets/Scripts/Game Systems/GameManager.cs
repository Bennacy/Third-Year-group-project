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
    public Camera bridgeCam;

    public Animator uiAnimator;
    public Image globalImage;
    public TextMeshProUGUI globalText;

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
    public List<Enemy> aliveEnemies;
    public int currentWave;
    public int maxWave;
    public bool hideUI;

    [Space(10)]
    [Header("Map Variables")]
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
            inGame = false;
            FadeInImage(.3f, null, Color.black);
            waitForFade = true;
        }
    }

    void NewScene(){
        if(GetPlayerController()){
            inGame = true;
            won = false;
            died = false;
            newHighScore = false;
            enemiesKilled = 0;
            time = 0;
            currency = 0;

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

    public void FadeInOutImage(float fadeTime, Sprite spriteToUse, Color colorToUse){
        globalImage.sprite = spriteToUse;
        globalImage.color = colorToUse;
        uiAnimator.speed = 1 / fadeTime;

        Debug.Log("Fade In-Out");
        uiAnimator.Play("Fade In-Out");
    }
    #endregion
}
