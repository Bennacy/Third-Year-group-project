using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthTest : MonoBehaviour, IHasHealth
{
    public int maxHealth {get; set;}
    public int health {get; set;}

    public void Damage(int damageVal)
    {
        health -= damageVal;
        // Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");
        if(health <= 0){
            Debug.Log("Killed!!!!");
            gameObject.SetActive(false);
        }
    }

    public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        Debug.Log(health);
    }

    public int enemyMaxHealth;

    void Awake()
    {
        health = maxHealth = enemyMaxHealth;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
