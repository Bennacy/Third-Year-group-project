
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public enum CharacterUpgrades{None, Health, Stamina}

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI LevelText;
    
    public int price;
    public WeaponScript weapon;
    public CharacterUpgrades upgrade;

    public float increaseBy;
    public int maxLevel;

    
    public void BuyUpgrade(){
        if(GameManager.Instance.score <= price){
            GameManager.Instance.score -= price;
        }

        price = Mathf.RoundToInt(price * 1.5f);

        if(upgrade != CharacterUpgrades.None){
            switch(upgrade){
                case CharacterUpgrades.Health:
                    GameManager.Instance.playerController.maxHealth += Mathf.RoundToInt(increaseBy);
                break;
                
                case CharacterUpgrades.Stamina:
                    GameManager.Instance.playerController.maxStamina += increaseBy;
                break;
            }

            return;
        }

        weapon.damage += increaseBy;
    }
}