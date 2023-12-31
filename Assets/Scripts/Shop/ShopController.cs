#if UNITY_EDITOR
using UnityEditor.Rendering.Universal;
using UnityEditor.UIElements;
#endif

using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public static ShopController instance;

    public Upgrade[] upgrades;

    public Text scoreText;
    
    public GameObject shopUI;

    public Transform shopContent;
    public GameObject itemPrefab;
    public GameObject item;

    public WeaponScript[] weapons;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            
                item = Instantiate(itemPrefab, shopContent);

                upgrade.itemRef = item;


                foreach (Transform child in item.transform)
                {
                    if (child.gameObject.name == "Cost")
                    {
                        child.gameObject.GetComponent<Text>().text = "Cost: " + upgrade.cost.ToString();
                    }
                    else if (child.gameObject.name == "Name")
                    {
                        child.gameObject.GetComponent<Text>().text = upgrade.name.ToString();
                    }
                    else if (child.gameObject.name == "Image")
                    {
                        child.gameObject.GetComponent<Image>().sprite = upgrade.image;
                    }
                }

                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    BuyUpgrade(upgrade);
                });
            

            

         
            
        }
       
    }


    private void Update()
    {
        foreach (Upgrade upgrade in upgrades) 
        {

            
                
                        if (upgrade.currentTier >= upgrade.maxTier)
                        {
                         upgrade.itemRef.transform.GetChild(2).GetComponent<Text>().text = "Cost: " + " MAX ";
                         upgrade.cost = int.MaxValue;
                        }
                    
                
            
        }


        
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        if (GameManager.Instance.score >= upgrade.cost)
        {
            GameManager.Instance.score -= upgrade.cost;
            upgrade.cost = Mathf.RoundToInt(upgrade.cost * 1.5f);
            upgrade.itemRef.transform.GetChild(2).GetComponent<Text>().text = "Cost: " + upgrade.cost.ToString();
            
                
            

            ApplyUpgrade(upgrade);
        }
    }


    public void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.name)
        {
            case "Hammer Upgrade":
                upgrade.currentTier += 1;
                weapons[0].damage *= 1.5f;
                    break;
            case "Sword Upgrade":
                upgrade.currentTier += 1;
                weapons[1].damage *= 1.5f;
                break;
            case "Axe Upgrade":
                upgrade.currentTier += 1;
                weapons[2].damage *= 1.5f;
                break;
            default:
                break;
        }
    }

    public void ToggleShop()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }

    private void  OnGUI()
    {
        
        scoreText.text = "Score: " + GameManager.Instance.score;
        
    }
}

[System.Serializable]
public class Upgrade
{
    public string name;
    public int cost;
    public int maxTier = 2;
    public int currentTier = 0;
    public Sprite image;
    [HideInInspector] public GameObject itemRef;

}
