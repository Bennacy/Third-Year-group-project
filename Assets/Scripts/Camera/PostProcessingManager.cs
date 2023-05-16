using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    private Vector4 gainVector;
    private float baseGain;
    private Vector4 shadowsVector;
    private float baseShadows;
    public Volume volume;
    private LiftGammaGain gain;
    private ShadowsMidtonesHighlights shadows;
    
    void Start()
    {
        volume.profile.TryGet<LiftGammaGain>(out gain);
        volume.profile.TryGet<ShadowsMidtonesHighlights>(out shadows);

        gainVector = gain.gain.value;
        baseGain = gainVector.w;
        shadowsVector = shadows.shadows.value;
        baseShadows = shadowsVector.w;
    }

    void Update()
    {
        gainVector.w = GameManager.Instance.brightness.Remap(0, 1, baseGain, 1);
        shadowsVector.w = GameManager.Instance.brightness.Remap(0, 1, baseShadows, 1);

        gain.gain.value = gainVector;
        shadows.shadows.value = shadowsVector;
    }
}