using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    public CanvasGroup group;
    public GameObject weaponsHighlight;
    public GameObject weaponsWrapper;
    public GameObject characterHighlight;
    public GameObject characterWrapper;

    public TextMeshProUGUI runesText;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            OpenShop();
        }

        if(!GameManager.Instance.paused && group.gameObject.activeSelf){
            SecondCloseShop();
        }

        runesText.text = "Runes: " + GameManager.Instance.currency;
    }

    public void OpenShop(){     
        StartCoroutine(OpenShopFade());
    }

    private IEnumerator OpenShopFade(){
        group.alpha = 0;
        group.gameObject.SetActive(true);
        GameManager.Instance.TogglePause();
        GameManager.Instance.shopOpen = true;
        ShowWeapons();

        while(group.alpha < 1){
            group.alpha += Time.unscaledDeltaTime*7.5f;
            yield return null;
        }
    }
    
    public void CloseShop(){
        group.gameObject.SetActive(false);
        GameManager.Instance.TogglePause();
        GameManager.Instance.shopOpen = false;
    }

    private void SecondCloseShop(){
        group.gameObject.SetActive(false);
        GameManager.Instance.shopOpen = false;
    }

    public void ShowWeapons(){
        weaponsHighlight.SetActive(true);
        weaponsWrapper.SetActive(true);

        characterHighlight.SetActive(false);
        characterWrapper.SetActive(false);
    }

    public void ShowCharacter(){
        characterHighlight.SetActive(true);
        characterWrapper.SetActive(true);

        weaponsHighlight.SetActive(false);
        weaponsWrapper.SetActive(false);
    }
}