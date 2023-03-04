using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthTest : MonoBehaviour, IHasHealth
{
    #region IHasHealth
    public int maxHealth {get; set;}
    public int health {get; set;}

    public void Damage(int damageVal)
    {
        health -= damageVal;
        // Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");
        if(health <= 0 && !dead){
            dead = true;
            animator.Play("Death");
            Debug.Log("Killed!!!!");
        }
    }

    public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        Debug.Log(health);
    }
    #endregion

    public int enemyMaxHealth;
    public Animator animator;
    public bool dead;

    void Awake()
    {
        health = maxHealth = enemyMaxHealth;
        dead = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die(){
        gameObject.SetActive(false);
    }
}
