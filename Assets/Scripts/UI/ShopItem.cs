
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public enum CharacterUpgrades{None, Health, Stamina}

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI priceText;
    
    public int price;
    public WeaponScript weapon;
    public CharacterUpgrades upgrade;

    public float increaseBy;
    private int level;
    public int maxLevel;

    void Start()
    {
        LevelText.text = "Level 0";
        priceText.text = "Price: " + price*100;
    }

    void Update()
    {
    }

    void OnEnable()
    {
        if(GameManager.Instance.currency < price*100){
            priceText.color = Color.red;
        }else{
            priceText.color = Color.white;
        }
    }

    
    public void BuyUpgrade(){
        if(level >= maxLevel)
            return;
        if(GameManager.Instance.currency < price*100)
            return;
        
        GameManager.Instance.totalUpgrades++;
        GameManager.Instance.currency -= price*100;
        price = Mathf.RoundToInt(price * 1.5f);

        level++;

        LevelText.text = "Level " + level;
        priceText.text = "Price: " + price*100;

        if(GameManager.Instance.currency < price*100){
            priceText.color = Color.red;
        }else{
            priceText.color = Color.white;
        }

        if(level >= maxLevel){
            LevelText.text = "Max Level";
            LevelText.color = Color.gray;
            priceText.text = "";
            priceText.color = Color.gray;
        }

            switch(upgrade){
                case CharacterUpgrades.Health:
                    GameManager.Instance.playerController.maxHealth += Mathf.RoundToInt(increaseBy);
                break;
                
                case CharacterUpgrades.Stamina:
                    GameManager.Instance.playerController.maxStamina += increaseBy;
                break;

                case CharacterUpgrades.None:
                    weapon.damage += increaseBy;
                break;
            }

    }
}