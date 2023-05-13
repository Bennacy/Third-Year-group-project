using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EZCameraShake;

public class PlayerController : MonoBehaviour, IHasHealth
{

    [Header("References")]
    public CameraController cameraController;
    private Rigidbody rb;
    private Vector2 moveDirection;
    private PlayerAnimatorHandler animatorHandler;
    public PlayerInput playerInput;
    public GameObject weapon;
    private WeaponHandler weaponHandler;
    public GameObject shield;
    public AudioSource audioSource;
    private CameraShake cameraShake;
    [Space(10)]

    [Header("Camera")]
    public Vector2 cameraVerticalBounds;
    private Vector2 cameraRotate;
    private float currCameraAngle;
    [Space(10)]

    [Header("Movement")]
    public bool onSlope;
    public bool grounded;
    public bool walking;
    public bool sprinting;
    public int gravityForce;
    public float walkSpeed;
    public float runSpeed;
    public float blockSpeed;
    public float moveSpeed;

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

    public TransitionScript transitionScript;

    void Awake()
    {
        animatorHandler = GetComponentInChildren<PlayerAnimatorHandler>();
        cameraShake = GetComponentInChildren<CameraShake>();
        audioSource = GetComponent<AudioSource>();
        weaponHandler = GetComponent<WeaponHandler>();
                
        currCameraAngle = 0;
        moveSpeed = walkSpeed;
        playerInput = GetComponent<PlayerInput>();

        controlCamAction = playerInput.actions["Look"];
        controlCamAction.performed += context => CameraInput(context);
        controlCamAction.canceled += context => CameraInput(context);

        sprintAction = playerInput.actions["Sprint"];
        sprintAction.performed += context => Sprint(context);
        sprintAction.canceled += context => Sprint(context);

        moveAction = playerInput.actions["Walk"];
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

        health = maxHealth = playerHealth;
    }

    void OnDisable()
    {
        controlCamAction.performed -= context => CameraInput(context);
        controlCamAction.canceled -= context => CameraInput(context);
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

        
        // float speedValue = moveSpeed;
        // if(blocking){
        //     speedValue = blockSpeed;
        // }

        // Vector3 flatVelocity = rb.velocity;
        // flatVelocity.y = 0;

        // if(flatVelocity.magnitude > speedValue){
        //     Vector3 limitedVelocity = flatVelocity.normalized * speedValue;
        //     limitedVelocity.y = rb.velocity.y;
        //     rb.velocity = limitedVelocity;
        // }
        
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 1.5f);
        // if(Physics.Raycast(transform.position, Vector3.down, out test, 1.5f)){
        //     Debug.Log(Vector3.Angle(test.normal, Vector3.up));
        //     if(test.normal != Vector3.up)
        //         onSlope = true;
        // }
    }

    void FixedUpdate()
    {
        
        float speedValue = moveSpeed;
        if(blocking){
            speedValue = blockSpeed;
        }


        RaycastHit groundCollision;
        if(Physics.Raycast(transform.position, Vector3.down, out groundCollision, 1.5f)){ // Touching the ground
            grounded = true;

            if(Vector3.Angle(groundCollision.normal, Vector3.up) > 5){
                onSlope = true;
            }else{
                onSlope = false;
            }
        }else{
            grounded = false;
        }
        
        if(!attacking){
            Vector3 direction = moveDirection.y * transform.forward + moveDirection.x * transform.right;
            if(onSlope){
                direction = Vector3.ProjectOnPlane(direction, groundCollision.normal);
            }

            rb.AddForce(direction * speedValue * 10, ForceMode.Force);


            // if(Physics.Raycast(transform.position, Vector3.down, out test, 1.5f)){ // Touching the ground

            //     Vector3 newDir = Vector3.ProjectOnPlane(vel, test.normal).normalized;

            //     if(Vector3.Angle(test.normal, Vector3.up) > 5){
            //         onSlope = true;
            //         rb.AddForce(newDir * speedValue * 15, ForceMode.Force);
            //     }else{
            //         onSlope = false;
            //         rb.AddForce(newDir * speedValue * 10, ForceMode.Force);
            //     }
            // }else{
            //     Debug.Log("Falling");
            //     rb.AddForce(transform.forward * 15, ForceMode.Force);
            // }
        }

        if(!grounded)
            rb.AddForce(Physics.gravity * gravityForce, ForceMode.Force);
    }

    void LateUpdate()
    {
        if(GameManager.Instance.paused)
            return;

        RotateCamera();
    }

    private void MovePlayer(InputAction.CallbackContext context){
        Vector2 inputVector = context.ReadValue<Vector2>();
        walking = context.performed;

        moveDirection = inputVector;
    }

    private void Sprint(InputAction.CallbackContext context){
        sprinting = context.performed;

        if(context.performed){
            moveSpeed = runSpeed;
        }else{
            moveSpeed = walkSpeed;
        }
    }

    private void CameraInput(InputAction.CallbackContext context){
        if(GameManager.Instance.paused)
            return;

        Vector2 inputVector = context.ReadValue<Vector2>();
        cameraRotate = inputVector;
    }
    private void RotateCamera(){
            Vector3 currPlayerRotation = transform.localRotation.eulerAngles;
            Vector3 currCamRotation = cameraController.transform.rotation.eulerAngles;

            float sens = cameraController.sens;
            if(playerInput.currentControlScheme != "Keyboard&Mouse"){
                sens *= 10;
            }

            Vector2 localRotate = cameraRotate;
            if(GameManager.Instance.invertX)
                localRotate.x *= -1;
            if(GameManager.Instance.invertY)
                localRotate.y *= -1;

            currPlayerRotation.y += localRotate.x * sens;
            currCameraAngle -= localRotate.y * sens;
            currCamRotation.y = 0;
            
            currCameraAngle = Mathf.Clamp(currCameraAngle, cameraVerticalBounds.x, cameraVerticalBounds.y);
            currCamRotation.x = currCameraAngle;

            cameraController.transform.localRotation = Quaternion.Euler(currCamRotation);
            transform.rotation = Quaternion.Euler(currPlayerRotation);
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
        // cameraShake.ShakeRotation(1f, 1f, 1f, .25f);
        CameraShaker.Instance.ShakeOnce(3f, 2f, .1f, .1f);

        if(blocking){
            float staminaDamage = damageVal * weaponHandler.currentWeapon.damageBlocked;
            damageVal -= Mathf.RoundToInt(staminaDamage);
            stamina -= staminaDamage;
            animatorHandler.SetTrigger("BlockRecoil");

            if(stamina <= 0){
                blocking = false;
                animatorHandler.EndBlock();
                staminaCooldown = 1.5f;
            }
        }

        health -= damageVal;

        if (health <= 0)
            GameManager.Instance.died = true;
        
    }

   public void Recover(int recoverVal)
    {
        health = Mathf.Clamp(health + recoverVal, 0, maxHealth);
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
