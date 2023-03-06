using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IHasHealth
{
    [Header("References")]
    public CameraController cameraController;
    private Rigidbody rb;
    private Vector2 moveDirection;
    private PlayerAnimatorHandler animatorHandler;
    public PlayerInput playerInput;
    public GameObject weapon;
    public GameObject shield;
    [Space(10)]

    [Header("Camera")]
    public Vector2 cameraVerticalBounds;
    private float currCameraAngle;
    [Space(10)]

    [Header("Movement")]
    public int gravityForce;
    public float walkSpeed;
    public float runSpeed;
    public float blockSpeed;
    private float moveSpeed;
    [SerializeField]
    [Range(0.1f, 5f)]
    private float positionHistoryDuration = 1f;
    [SerializeField]
    [Range(0.001f, 1f)]
    private float positionHistoryInterval = 0.1f;

    private Queue<Vector3> velocityHistory;
    private float lastPositionTime;
    private int maxQueueSize;

    [Space(10)]

    [Header("Combat")]
    public bool attacking;
    public bool blocking;
    [Space(10)]

    [Header("PlayerStats")]

    public int playerHealth = 250;
    public int maxHealth { get; set; }
    public int health { get; set; }

    // [Header("Input")]
    // private InputAction moveAction;
    // private InputAction controlCamAction;
    // private InputAction sprintAction;
    // private InputAction attackAction;
    // private InputAction blockAction;

    void Awake()
    {
        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
                
        currCameraAngle = 0;
        moveSpeed = walkSpeed;
        playerInput = GetComponent<PlayerInput>();

        InputAction controlCamAction = playerInput.actions["Look"];
        controlCamAction.performed += context => MoveCamera(context);

        InputAction sprintAction = playerInput.actions["Sprint"];
        sprintAction.performed += context => Sprint(context);
        sprintAction.canceled += context => Sprint(context);

        InputAction moveAction = playerInput.actions["Move"];
        moveAction.performed += context => MovePlayer(context);
        moveAction.canceled += context => MovePlayer(context);

        InputAction attackAction = playerInput.actions["Attack"];
        attackAction.performed += context => Attack(context);

        InputAction blockAction = playerInput.actions["Block"];
        blockAction.performed += context => Block(context);
        blockAction.canceled += context => Block(context);

        InputAction pauseAction = playerInput.actions["Pause"];
        pauseAction.performed += context => GameManager.Instance.TogglePause();

        InputAction unpauseAction = playerInput.actions["Unpause"];
        unpauseAction.performed += context => GameManager.Instance.TogglePause();

        maxQueueSize = Mathf.CeilToInt(1f / positionHistoryInterval * positionHistoryDuration);
        velocityHistory = new Queue<Vector3>(maxQueueSize);

        health = maxHealth = playerHealth;
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

        if(lastPositionTime + positionHistoryInterval <= Time.time)
        {
            if(velocityHistory.Count == maxQueueSize)
            {
                velocityHistory.Dequeue();
            }

            velocityHistory.Enqueue(rb.velocity);
            lastPositionTime = Time.time;
        }
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


    public Vector3 AverageVelocity
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach(Vector3 velocity in velocityHistory)
            {
                average += velocity;
            }
            average.y = 0;

            return average / velocityHistory.Count;
        }

        
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
        if(GameManager.Instance.paused)
            return;

        Vector2 inputVector = context.ReadValue<Vector2>();
        float sens = cameraController.sens;

        if(playerInput.currentControlScheme == "Gamepad"){
            sens *= 100;
        }

        if(context.performed){
            Vector3 currPlayerRotation = transform.localRotation.eulerAngles;
            Vector3 currCamRotation = cameraController.transform.rotation.eulerAngles;

            currPlayerRotation.y += inputVector.x * sens;
            currCameraAngle -= inputVector.y * sens;
            currCamRotation.y = 0;
            
            currCameraAngle = Mathf.Clamp(currCameraAngle, cameraVerticalBounds.x, cameraVerticalBounds.y);
            currCamRotation.x = currCameraAngle;

            cameraController.transform.localRotation = Quaternion.Euler(currCamRotation);
            transform.rotation = Quaternion.Euler(currPlayerRotation);
        }
    }

    private void Attack(InputAction.CallbackContext context){
        if(blocking || GameManager.Instance.paused)
            return;

        if(!attacking || animatorHandler.nextInCombo){ // If the player is not attacking at all, or if the combo window is open
            animatorHandler.StartAttack();
            return;
        }
    }

    private void Block(InputAction.CallbackContext context){
        if(attacking || GameManager.Instance.paused)
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

    void IHasHealth.Damage(int damageVal)
    {
        health -= damageVal;
        Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");
        if (health <= 0)
        {
            Debug.Log("Killed!!!!");
            gameObject.SetActive(false);
        }
    }

    void IHasHealth.Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        Debug.Log(health);
    }
}
