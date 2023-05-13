using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [SerializeField] private bool enable;

    [SerializeField][Range(0,0.1f)] private float amplitude;
    [SerializeField][Range(0,30)] private float frequency;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform armsCamera;
    [SerializeField] private Transform cameraHolder;
    private Rigidbody playerRigidbody;

    private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        playerRigidbody = playerController.GetComponent<Rigidbody>();
        startPos = mainCamera.localPosition;
    }

    private Vector3 FootstepMotion(){
        Vector3 pos = Vector3.zero;
        pos.x = Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x = Mathf.Cos(Time.time * frequency / 2) * amplitude*2;
        return pos;
    }

    private void CheckMotion(){
        float speed = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z).magnitude;

        if(speed < toggleSpeed) return;
        if(!playerController.grounded) return;

        PlayMotion(FootstepMotion());
    }

    private void PlayMotion(Vector3 motion){
        mainCamera.localPosition += motion; 
        armsCamera.localPosition += motion; 
    }
    
    private void ResetPosition(){
        if(mainCamera.localPosition == startPos) return;

        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, startPos, Time.deltaTime);
        armsCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, startPos, Time.deltaTime);
    }

    private Vector3 FocusTarget(){
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + mainCamera.localPosition.y, transform.position.z);
        pos += mainCamera.forward * 15;
        return pos;
    }

    void Update(){
        if(!enable) return;

        CheckMotion();
        // mainCamera.LookAt(FocusTarget());
    }
}
