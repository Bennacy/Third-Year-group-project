using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Game Systems/Wave")]
public class WaveScript : ScriptableObject
{
    public List<WaveInfo> waveSequence;
}

[System.Serializable]
public class WaveInfo
{
    public GameObject prefab;
    public int count;
    public float waitBetween;
    public float waitAfter;
}
