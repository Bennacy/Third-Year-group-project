using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HealthDespawn());
    }

    private IEnumerator HealthDespawn()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    
}
