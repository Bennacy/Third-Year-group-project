using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Camera cam;
    public Transform camTransform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = camTransform.rotation.eulerAngles;
        rotation.y = 0;
        camTransform.rotation = Quaternion.Euler(rotation);
    }
}
