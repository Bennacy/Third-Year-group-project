
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShopController.instance.ToggleShop();
            GameManager.Instance.TogglePause();
        }
    }

    private void OnTriggerExit(Collider other)
    {
       ShopController.instance.ToggleShop();
        
    }
}
