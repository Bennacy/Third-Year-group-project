using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HealthDespawn());
        playerController = GetComponent<PlayerController>();
    }

    private IEnumerator HealthDespawn()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    

}
