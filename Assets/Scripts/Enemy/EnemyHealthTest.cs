using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthTest : MonoBehaviour, IDamageable
{
    public int health {get; set;}

    public void Damage(int damageVal)
    {
        health -= damageVal;
        Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");
        if(health <= 0){
            Debug.Log("Killed!!!!");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = 200;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
