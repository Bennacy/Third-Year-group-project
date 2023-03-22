using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasHealth
{
    int maxHealth {get; set;}
    int health {get; set;}
    void Damage(int damageVal);
    void Recover(int recoverVal);   
}
