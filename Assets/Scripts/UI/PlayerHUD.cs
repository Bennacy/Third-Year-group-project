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


    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<IHasHealth>();

        InitializeHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    void InitializeHealthBar(){
        healthBar_TargetWidth = healthBar_MaxWidth = healthBar_Transform.sizeDelta.x;
    }

    void UpdateHealthBar(){
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = GameManager.Instance.mainCam.transform.rotation.eulerAngles.y + 180;
        canvas.transform.rotation = Quaternion.Euler(rotation);
        
        if(playerHealth.health < healthBar_PreviousHealth){
            StartCoroutine(UpdateHealthBarWidth());
        }
        
        Vector2 newSize = healthBar_Transform.sizeDelta;
        newSize.x = Mathf.Lerp(newSize.x, healthBar_TargetWidth, 5*Time.deltaTime);
        healthBar_Transform.sizeDelta = newSize;

        healthBar_PreviousHealth = playerHealth.health;
    }
    
    IEnumerator UpdateHealthBarWidth(){
        healthBar_TargetWidth = (playerHealth.health*healthBar_MaxWidth) / playerHealth.maxHealth;
        for(int i = 0; i < 4; i++){
            healthBar_Front.color = (i % 2 == 0) ? healthBar_FlashColor : healthBar_FrontColor;
            yield return new WaitForSeconds(.05f);
        }
    }
}
