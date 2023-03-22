using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public Canvas canvas;
    private IHasHealth playerHealth;
    public Image healthBar_Front;
    public Image healthBar_Background;
    public Color healthBar_BackColor;
    public Color healthBar_FrontColor;
    public Color healthBar_FlashColor;
    public RectTransform healthBar_Transform;
    private float healthBar_MaxWidth;
    private float healthBar_TargetWidth;
    private int healthBar_PreviousHealth;
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
        healthBar_TargetWidth = healthBar_MaxWidth = healthBar_Transform.sizeDelta.x;
    }

    void UpdateHealthBar(){
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = GameManager.Instance.mainCam.transform.rotation.eulerAngles.y + 180;
        canvas.transform.rotation = Quaternion.Euler(rotation);
        
        if(playerHealth.health < healthBar_PreviousHealth){
            StartCoroutine(FlashHeathBar());
        }
        
        healthBar_TargetWidth = (playerHealth.health*healthBar_MaxWidth) / playerHealth.maxHealth;
        Vector2 newSize = healthBar_Transform.sizeDelta;
        newSize.x = Mathf.Lerp(newSize.x, healthBar_TargetWidth, 5*Time.deltaTime);
        healthBar_Transform.sizeDelta = newSize;

        healthBar_PreviousHealth = playerHealth.health;
    }
    
    IEnumerator FlashHeathBar(){
        for(int i = 0; i < 4; i++){
            healthBar_Front.color = (i % 2 == 0) ? healthBar_FlashColor : healthBar_FrontColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
