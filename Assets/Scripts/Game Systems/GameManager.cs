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

    public HighScores highScores;
    public PersonalScore savedScore;

    public bool paused;
    public bool shopOpen;
    public bool won;
    public bool died;
    public float time;
    public float highScore;
    public int currency;
    public int totalUpgrades;
    public int enemiesKilled;
    public bool newHighScore;
    public bool inGame;
    public bool loadingScene;
    public bool betweenWaves;
    public int score;
    public int kills;
    public List<Enemy> aliveEnemies;
    public int currentWave;
    public int maxWave;
    public int leaderboardPlacement;

    public int FOV;
    public float sensitivity;
    public bool invertX;
    public bool invertY;

    public WeaponScript[] weapons;

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
        currentWave = 1;
        enemiesKilled = 0;
        time = 0;
        currency = 0;
        score = 0;
        totalUpgrades = 0;
    }

    void OnEnable()
    {
        LoadSettings();
        SceneManager.sceneLoaded += delegate{NewScene();};

        SaveSystem.Init();

        LoadScores();
    }

    void Update()
    {
        if(inGame && !betweenWaves){
            time += Time.deltaTime;
        }
        
        if(!loadingScene && inGame){
            if(won){
                if(highScore > 0 && time < highScore){
                    newHighScore = true;
                    highScore = time;
                }
                inGame = false;
                // SaveScores();
                savedScore = new PersonalScore(score, Mathf.RoundToInt(time), kills);
                leaderboardPlacement = highScores.IsInLeaderboard(savedScore);
                
                LoadScene("Victory Screen");
            }
            if(died){
                inGame = false;
                // SaveScores();

                savedScore = new PersonalScore(score, Mathf.RoundToInt(time), kills);
                leaderboardPlacement = highScores.IsInLeaderboard(savedScore);
                LoadScene("Death Screen");
            }
        }
    }

    void NewScene(){
        Time.timeScale = 1;
        paused = false;
        won = false;
        died = false;
        newHighScore = false;
        betweenWaves = false;
        AudioListener.pause = false;
        
        if(GetPlayerController()){
            inGame = true;
            currentWave = 1;
            enemiesKilled = 0;
            time = 0;
            currency = 0;
            score = 0;
            totalUpgrades = 0;
            
            foreach(WeaponScript weapon in weapons)
            {
                weapon.damage = weapon.defaultDamage;
            }

            InputAction nextWave = playerInput.actions["New Wave"];
            nextWave.performed += context => {
                if(betweenWaves)
                    betweenWaves = false;
            };
            return;
        }

        inGame = false;
    }

    void OnApplicationQuit()
    {
        SaveScores();
        SaveSettings();
    }

    bool GetPlayerController(){
        playerController = FindObjectOfType<PlayerController>();
        if(playerController != null){
            playerInput = playerController.playerInput;
            mainCam = playerController.cameraController.mainCam;
        }

        return playerController != null;
    }

    public void TogglePause(){
        paused = !paused;
        AudioListener.pause = paused;
        
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


    //! =============== Save Functions ===============
    public void SaveScores(){
        PersonalScore personalScore = new PersonalScore(score, Mathf.RoundToInt(time), kills);
        leaderboardPlacement = highScores.InsertScore(personalScore);
        // Debug.Log(leaderboardPlacement);
        string saving = JsonUtility.ToJson(highScores, true);
        SaveSystem.Save(saving, "High Scores");
    }
    public void SaveScores(PersonalScore personalScore){
        leaderboardPlacement = highScores.InsertScore(personalScore);
        // Debug.Log(leaderboardPlacement);
        string saving = JsonUtility.ToJson(highScores, true);
        SaveSystem.Save(saving, "High Scores");
    }
    public void LoadScores(){
        highScores = new HighScores();
        string scoreString = SaveSystem.Load("High Scores");
        if(scoreString != null)
            highScores = JsonUtility.FromJson<HighScores>(scoreString);
    }
    public void ClearSave(){
        highScores = new HighScores();
        string saving = JsonUtility.ToJson(highScores, true);
        SaveSystem.Save(saving, "High Scores");
    }

    public void SaveSettings(){
        
        Settings saveSettings = new Settings(
            AudioManager.Instance.masterVolume,
            AudioManager.Instance.sfxVolume,
            AudioManager.Instance.musicVolume,
            FOV,
            QualitySettings.GetQualityLevel(),
            invertX,
            invertY
        );

        string saving = JsonUtility.ToJson(saveSettings, true);
        SaveSystem.Save(saving, "Settings");
    }
    public void LoadSettings(){
        string settingString = SaveSystem.Load("Settings");
        if(settingString != null){
            Settings loadedSettings = JsonUtility.FromJson<Settings>(settingString);

            AudioManager.Instance.SetMasterVolume(loadedSettings.masterVolume);
            AudioManager.Instance.SetSFXVolume(loadedSettings.sfxVolume);
            AudioManager.Instance.SetMusicVolume(loadedSettings.musicVolume);
            FOV = loadedSettings.fov;
            QualitySettings.SetQualityLevel(loadedSettings.graphicsQuality);
            invertX = loadedSettings.invertX;
            invertY = loadedSettings.invertY;
        }else{
            
            AudioManager.Instance.SetMasterVolume(.5f);
            AudioManager.Instance.SetSFXVolume(.5f);
            AudioManager.Instance.SetMusicVolume(.5f);
            FOV = 90;
            QualitySettings.SetQualityLevel(2);
        }
    }
    //! =============== Save Functions ===============


    public void Quit(){
        Application.Quit();
    }

    public void LoadScene(string sceneName){
        // SceneManager.LoadScene(sceneName);
        // Debug.Log("Normal - Loading scene " + sceneName);
        StartCoroutine(LoadSceneWait(sceneName));
    }

    private IEnumerator LoadSceneWait(string sceneName){
        // uiAnimator.Play("Fade In");
        loadingScene = true;

        Debug.Log("Coroutine - Loading scene " + sceneName);
        float test = 0;
        while(test < 1){
            test += Time.unscaledDeltaTime;
            if(test >= 0.3f){
                loadingScene = false;
            }
            yield return null;
        }
        // yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);

        Debug.Log("Loaded scene " + sceneName);
    }
}
