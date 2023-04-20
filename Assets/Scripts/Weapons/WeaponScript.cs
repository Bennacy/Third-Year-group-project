using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Player")]
public class WeaponScript : ScriptableObject
{
    [Header("Info")]
    public string weaponName;
    public GameObject prefab;
    public Sprite icon;
    public bool leftHand;
    [Space(10)]

    [Header("Animations")]
    public string weaponType;
    public string[] attacks;
    [Space(10)]

    [Header("Audio")]
    public AudioClip equipClip;
    public AudioClip impactClip;
    public AudioClip[] swingClips;
    [Space(10)]

    [Header("Stats")]
    public float damage;
    public float defaultDamage;
}
