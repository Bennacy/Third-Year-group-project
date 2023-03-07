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

    public bool paused;
    public bool won;
    public float time;
    public int currency;
    public int enemiesKilled;
    public bool inGame;
    public bool fading;
    public bool waitForFade;
    
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
        // GetPlayerController();
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
                won = false;
            }
        }else{

        }


        if(won && inGame && !waitForFade){

            FadeInImage(1, null, Color.black);
            waitForFade = true;
        }
    }

    void NewScene(){
        if(GetPlayerController()){
            inGame = true;
            enemiesKilled = 0;
            time = 0;
            currency = 0;
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
