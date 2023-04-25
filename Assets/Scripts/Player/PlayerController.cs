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
    public AudioSource audioSource;
    private CameraShake cameraShake;
    [Space(10)]

    [Header("Camera")]
    public Vector2 cameraVerticalBounds;
    private float currCameraAngle;
    [Space(10)]

    [Header("Movement")]
    public bool walking;
    public bool sprinting;
    public int gravityForce;
    public float walkSpeed;
    public float runSpeed;
    public float blockSpeed;
    public float moveSpeed;
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
    public float maxStamina;
    public float staminaRegeneration;
    public float staminaCooldown;
    public float stamina;
    [Space(10)]

    [Header("Input")]
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private InputAction moveAction;
    private InputAction controlCamAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private InputAction blockAction;
    private InputAction pauseAction;
    private InputAction unpauseAction;

    void Awake()
    {
        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        cameraShake = GetComponentInChildren<CameraShake>();
        audioSource = GetComponent<AudioSource>();
                
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

        pauseAction = playerInput.actions["Pause"];
        pauseAction.performed += context => GameManager.Instance.TogglePause();

        unpauseAction = playerInput.actions["Unpause"];
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
        staminaCooldown -= Time.deltaTime;

        if(!blocking && staminaCooldown <= 0){
            stamina = Mathf.Clamp(stamina + staminaRegeneration*Time.deltaTime, 0, maxStamina);
        }

        if(!attacking && blocking){
            animatorHandler.StartBlock();
        }else{
            animatorHandler.EndBlock();
        }
        
        // Debug.Log(playerInput.currentControlScheme);

        // if(lastPositionTime + positionHistoryInterval <= Time.time)
        // {
        //     if(velocityHistory.Count == maxQueueSize)
        //     {
        //         velocityHistory.Dequeue();
        //     }

        // // Debug.DrawRay(transform.position, transform.forward, Color.red, 100);
        //     velocityHistory.Enqueue(rb.velocity);
        //     lastPositionTime = Time.time;
        // }


        // if(Input.GetKeyDown(KeyCode.Space)){
        //     playerInput.SwitchCurrentActionMap("Rebinding");

        //     rebindingOperation = moveAction.PerformInteractiveRebinding()
        //         .WithControlsExcluding("Mouse")
        //         .OnMatchWaitForAnother(.1f)
        //         .WithCancelingThrough("<Keyboard>/P")
        //         .OnComplete(operation => {
        //             Debug.Log("Done");
        //             playerInput.SwitchCurrentActionMap("In-Game");
        //             rebindingOperation.Dispose();
        //         })
        //         .Start();
        // }
    }

    void FixedUpdate()
    {
        float speedValue = moveSpeed;
        if(blocking){
            speedValue = blockSpeed;
        }
        
        if(!attacking){
            Vector3 gravity = new Vector3(0, rb.velocity.y, 0);
            Vector3 vel = moveDirection.y * transform.forward + moveDirection.x * transform.right;
            vel *= speedValue;
            rb.velocity = ((vel + gravity));
        }
        
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

        walking = context.performed;

        moveDirection = new Vector2(inputVector.x, inputVector.y);
    }

    private void Sprint(InputAction.CallbackContext context){
        sprinting = context.performed;

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
            // cameraShake.ShakeRotation(1f, 1f, 1f, .25f);
            return;
        }
    }

    private void Block(InputAction.CallbackContext context){
        if(GameManager.Instance.paused)
            return;
        
        if(context.performed && stamina > maxStamina / 10){
            blocking = true;
            // animatorHandler.StartBlock();
        }
        if(context.canceled){
            blocking = false;
            // animatorHandler.EndBlock();
        }
    }
    
    public IEnumerator HitStop(){
        animatorHandler.animator.enabled = false;
        // agent.updatePosition = false;
        yield return new WaitForSeconds(.1f);
        animatorHandler.animator.enabled = true;
        // agent.updatePosition = true;
    }
    public void Damage(int damageVal)
    {
        cameraShake.ShakeRotation(1f, 1f, 1f, .25f);

        if(blocking){
            stamina -= damageVal;
            animatorHandler.SetTrigger("BlockRecoil");

            if(stamina <= 0){
                blocking = false;
                animatorHandler.EndBlock();
                staminaCooldown = 1.5f;
            }
        }else{
            health -= damageVal;
            // Debug.Log("Took " + damageVal + " damage! (" + (health + damageVal) + " -> " + health + ")");

            if (health <= 0)
                GameManager.Instance.died = true;
        }
    }

   public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
        // Debug.Log(health);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Health")
        {
            if(health < 250) 
            {
                Recover(25);
                Destroy(other.gameObject);
            }
        }
    }
}
