using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCam;
    public Camera characterCam;

    public float FOV = 90;
    public float sens;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FOV = GameManager.Instance.FOV;
        
        mainCam.fieldOfView = FOV;
        characterCam.fieldOfView = FOV;
    }
}
