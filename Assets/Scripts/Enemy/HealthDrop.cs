using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public PlayerController playerController;   

    void Start()
    {
        StartCoroutine(HealthDespawn());
    }

    private IEnumerator HealthDespawn()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}