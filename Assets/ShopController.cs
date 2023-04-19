
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ShopController.instance.ToggleShop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ShopController.instance.ToggleShop();
    }
}
