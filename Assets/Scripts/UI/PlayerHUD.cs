using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Canvas canvas;
    private IHasHealth playerHealth;
    public Image healthBar;
    public Color healthBar_BackColor;
    public Color healthBar_FrontColor;
    public Color healthBar_FlashColor;
    private int healthBar_PreviousHealth;
    private float healthBar_targetFill;
    private bool hidingHUD;

    public TextMeshProUGUI enemiesRemaining;
    public TextMeshProUGUI waveCount;


    public void ToggleHUD(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(!hidingHUD);
        }
        // healthBar_Front.enabled = !hidingHUD;
        // healthBar_Background.enabled = !hidingHUD;
        // enemiesRemaining.enabled = !hidingHUD;
        // waveCount.enabled = !hidingHUD;
    }

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<IHasHealth>();

        InitializeHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
        enemiesRemaining.text = "Enemies remaining: " + GameManager.Instance.aliveEnemies.Count;
        waveCount.text = "Wave " + GameManager.Instance.currentWave + "/" + GameManager.Instance.maxWave;

        if(GameManager.Instance.hideUI != hidingHUD){
            hidingHUD = GameManager.Instance.hideUI;
            ToggleHUD();
        }
    }

    void InitializeHealthBar(){
        healthBar_targetFill = (float)playerHealth.health / (float)playerHealth.maxHealth;
    }

    void UpdateHealthBar(){
        // Vector3 rotation = transform.rotation.eulerAngles;
        // rotation.y = GameManager.Instance.mainCam.transform.rotation.eulerAngles.y + 180;
        // canvas.transform.rotation = Quaternion.Euler(rotation);
        
        if(playerHealth.health < healthBar_PreviousHealth){
            StartCoroutine(FlashHeathBar());
        }
        
        healthBar_targetFill = (float)playerHealth.health / (float)playerHealth.maxHealth;
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, healthBar_targetFill, 5*Time.deltaTime);

        healthBar_PreviousHealth = playerHealth.health;
    }
    
    IEnumerator FlashHeathBar(){
        for(int i = 0; i < 4; i++){
            healthBar.color = (i % 2 == 0) ? healthBar_FlashColor : healthBar_FrontColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
