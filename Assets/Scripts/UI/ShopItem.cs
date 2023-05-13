
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
        LevelText.text = "Current: " + DisplayValue().ToString();
        priceText.text = "Upgrade: " + price*10;
    }

    void Update()
    {
    }

    void OnEnable()
    {
        if(GameManager.Instance.currency < price*10){
            priceText.color = Color.red;
        }else{
            priceText.color = Color.white;
        }
    }

    
    public void BuyUpgrade(){
        AudioManager.Instance.PlayUIClick();
        if(level >= maxLevel)
            return;
        if(GameManager.Instance.currency < price*10)
            return;
        
        GameManager.Instance.totalUpgrades++;
        GameManager.Instance.currency -= price*10;
        price = Mathf.RoundToInt(price * 1.2f);

        level++;

        if(GameManager.Instance.currency < price*10){
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

        LevelText.text = "Current: " + DisplayValue().ToString();
        priceText.text = "Upgrade: " + price*10;
    }

    int DisplayValue(){
        switch(upgrade){
            case CharacterUpgrades.Health:
                return GameManager.Instance.playerController.maxHealth;
                
            case CharacterUpgrades.Stamina:
                return Mathf.RoundToInt(GameManager.Instance.playerController.maxStamina);

            default:
                return Mathf.RoundToInt(weapon.damage);
        }
    }
}