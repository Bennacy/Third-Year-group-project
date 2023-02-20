using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public float cameraSens;
    public Vector2 cameraVerticalBounds;
    public int gravityForce;
    public float walkSpeed;
    public float runSpeed;
    public float blockSpeed;
    private float moveSpeed;

    public bool attacking;
    public bool blocking;

    private float currCameraAngle;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction controlCamAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private InputAction blockAction;

    private Rigidbody rb;
    private Vector2 moveDirection;
    private PlayerAnimatorHandler animatorHandler;

    void Awake()
    {

        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
                
        currCameraAngle = 0;
        moveSpeed = walkSpeed;
        playerInput = GetComponent<PlayerInput>();

        controlCamAction = playerInput.actions["Look"];
        controlCamAction.performed += context => MoveCamera(context);

        sprintAction = playerInput.actions["Sprint"];
        sprintAction.performed += context => Sprint(context);
        sprintAction.canceled += context => Sprint(context);

        moveAction = playerInput.actions["Move"];
        moveAction.performed += context => MovePlayer(context);
        moveAction.canceled += context => MovePlayer(context);

        attackAction = playerInput.actions["Attack"];
        attackAction.performed += context => Attack(context);

        blockAction = playerInput.actions["Block"];
        blockAction.performed += context => Block(context);
        blockAction.canceled += context => Block(context);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Debug.Log(playerInput.currentControlScheme);
    }

    void FixedUpdate()
    {
        float speedValue = moveSpeed;
        if(blocking){
            speedValue = blockSpeed;
        }
        
        Vector3 gravity = new Vector3(0, rb.velocity.y, 0);
        Vector3 vel = moveDirection.y * transform.forward + moveDirection.x * transform.right;
        vel *= speedValue;
        rb.velocity = ((vel + gravity));
        
        rb.AddForce(Physics.gravity * gravityForce);
    }


    private void MovePlayer(InputAction.CallbackContext context){
        Vector2 inputVector = context.ReadValue<Vector2>();

        // if(context.performed){
        //     Debug.Log(inputVector);
        // }
        // if(context.canceled){
        //     Debug.Log("Canceled");
        // }

        moveDirection = new Vector2(inputVector.x, inputVector.y);
    }

    private void Sprint(InputAction.CallbackContext context){
        if(context.performed){
            moveSpeed = runSpeed;
        }else{
            moveSpeed = walkSpeed;
        }
    }

    private void MoveCamera(InputAction.CallbackContext context){
        Vector2 inputVector = context.ReadValue<Vector2>();
        float sens = cameraSens;

        if(playerInput.currentControlScheme == "Gamepad"){
            sens *= 100;
        }

        if(context.performed){
            Vector3 currPlayerRotation = transform.localRotation.eulerAngles;
            Vector3 currCamRotation = cameraTransform.rotation.eulerAngles;

            currPlayerRotation.y += inputVector.x * sens;
            currCameraAngle -= inputVector.y * sens;
            currCamRotation.y = 0;
            
            currCameraAngle = Mathf.Clamp(currCameraAngle, cameraVerticalBounds.x, cameraVerticalBounds.y);
            currCamRotation.x = currCameraAngle;

            cameraTransform.localRotation = Quaternion.Euler(currCamRotation);
            transform.rotation = Quaternion.Euler(currPlayerRotation);
        }
    }

    private void Attack(InputAction.CallbackContext context){
        if(blocking)
            return;

        if(!attacking)
            animatorHandler.StartAttack();
    }

    private void Block(InputAction.CallbackContext context){
        if(attacking)
            return;
        
        if(context.performed){
            blocking = true;
            animatorHandler.StartBlock();
        }
        if(context.canceled){
            blocking = false;
            animatorHandler.EndBlock();
        }
    }
}
