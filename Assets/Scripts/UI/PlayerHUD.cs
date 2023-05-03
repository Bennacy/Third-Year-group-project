using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Canvas canvas;
    private PlayerController player;
    public Image healthBar;
    public Image staminaBar;
    public Color healthBar_BackColor;
    public Color healthBar_FrontColor;
    public Color healthBar_FlashColor;
    private int healthBar_PreviousHealth;
    private float healthBar_targetFill;

    public Image shopImage;
    public Sprite[] shopIcons;
    public Image waveImage;
    public Sprite[] waveIcons;

    public TextMeshProUGUI enemiesRemaining;
    public TextMeshProUGUI waveCount;
    private PlayerInput playerInput;

    private Animator animator;
    private bool playedNoEnemies;


    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerInput = player.playerInput;

        animator = GetComponent<Animator>();

        InitializeHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
        enemiesRemaining.text = GameManager.Instance.aliveEnemies.Count.ToString();
        waveCount.text = "Wave " + GameManager.Instance.currentWave + "/" + GameManager.Instance.maxWave;

        float staminaBar_targetFill = (float)player.stamina / (float)player.maxStamina;
        staminaBar.fillAmount = Mathf.Lerp(staminaBar.fillAmount, staminaBar_targetFill, 5*Time.deltaTime);

        if(GameManager.Instance.aliveEnemies.Count == 0 && !playedNoEnemies){
            animator.SetTrigger("No Enemies");
            playedNoEnemies = true;
        }

        if(GameManager.Instance.aliveEnemies.Count != 0 && playedNoEnemies){
            animator.SetTrigger("Hide Prompt");
            playedNoEnemies = false;
        }

        switch(playerInput.currentControlScheme){
            case "Keyboard":
                shopImage.sprite = shopIcons[0];
                waveImage.sprite = waveIcons[0];
                break;
            case "xBox":
                shopImage.sprite = shopIcons[1];
                waveImage.sprite = waveIcons[1];
                break;
            case "PS4":
                shopImage.sprite = shopIcons[2];
                waveImage.sprite = waveIcons[2];
                break;
        }
    }

    void InitializeHealthBar(){
        healthBar_targetFill = (float)player.health / (float)player.maxHealth;
    }

    void UpdateHealthBar(){
        // Vector3 rotation = transform.rotation.eulerAngles;
        // rotation.y = GameManager.Instance.mainCam.transform.rotation.eulerAngles.y + 180;
        // canvas.transform.rotation = Quaternion.Euler(rotation);
        
        if(player.health < healthBar_PreviousHealth){
            StartCoroutine(FlashHeathBar());
        }
        
        healthBar_targetFill = (float)player.health / (float)player.maxHealth;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthBar_targetFill, 5*Time.deltaTime);

        healthBar_PreviousHealth = player.health;
    }

    // void UpdateStaminaBar(){
    //     // Vector3 rotation = transform.rotation.eulerAngles;
    //     // rotation.y = GameManager.Instance.mainCam.transform.rotation.eulerAngles.y + 180;
    //     // canvas.transform.rotation = Quaternion.Euler(rotation);
        
    //     if(player.health < healthBar_PreviousHealth){
    //         StartCoroutine(FlashHeathBar());
    //     }
        

    //     healthBar_PreviousHealth = player.health;
    // }
    
    IEnumerator FlashHeathBar(){
        for(int i = 0; i < 4; i++){
            healthBar.color = (i % 2 == 0) ? healthBar_FlashColor : healthBar_FrontColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
