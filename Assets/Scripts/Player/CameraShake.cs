using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float posDuration, rotDuration;
    public Vector3 origPos;
    public Quaternion origRot;
    public float xPos, yPos, zPos;
    public float xRot, yRot, zRot;
    public bool resetPos, resetRot;
    
    void Start()
    {
        origPos = Vector3.zero;
        origRot = Quaternion.identity;

        // ShakeRotation(.1f, .1f, .1f, 10);
    }

    void Update()
    {
        
    }

    void LateUpdate(){
        Shake();
    }

    public void Shake(){
        if(posDuration > 0){
            resetPos = false;
            posDuration -= Time.fixedDeltaTime;
            Vector3 random = Random.insideUnitSphere;
            float newX = random.x * xPos;
            float newY = random.y * yPos;
            float newZ = random.z * zPos;
            cameraTransform.localPosition = origPos + new Vector3(newX, newY, newZ);
            transform.localPosition = origPos + new Vector3(newX, newY, newZ);
        }else if(!resetPos){
            resetPos = true;
            cameraTransform.localPosition = origPos;
            transform.localPosition = origPos;
            xPos = yPos = zPos = 0;
        }

        if(rotDuration > 0){
            resetRot = false;
            rotDuration -= Time.fixedDeltaTime;
            Vector3 random = Random.insideUnitSphere;
            float newX = random.x * xRot;
            float newY = random.y * yRot;
            float newZ = random.z * zRot;
            cameraTransform.localRotation = Quaternion.Euler(origRot.eulerAngles + new Vector3(newX, newY, newZ));
            transform.localRotation = Quaternion.Euler(origRot.eulerAngles + new Vector3(newX, newY, newZ));
        }else if(!resetRot){
            resetRot = true;
            cameraTransform.localRotation = origRot;
            transform.localRotation = origRot;
            xRot = yRot = zRot = 0;
        }
    }

    public void ShakePosition(float xMag, float yMag, float zMag, float duration){
        origPos = cameraTransform.localPosition;
        origPos = transform.localPosition;
        xPos = xMag;
        yPos = yMag;
        zPos = zMag;
        posDuration = duration;
    }

    public void ShakeRotation(float xMag, float yMag, float zMag, float duration){
        origRot = cameraTransform.localRotation;
        origRot = transform.localRotation;
        xRot = xMag;
        yRot = yMag;
        zRot = zMag;
        rotDuration = duration;
    }
}
