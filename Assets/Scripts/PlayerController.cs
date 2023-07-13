using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private AudioSource source;
    private Animator animator;
    private Rigidbody rb;
    private BoxCollider bCollider;

    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public bool viewChanged = false;

    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Climbing,
        Air
    }

    public enum RotationState
    {
        Zero,
        Ninety,
        HunEighty,
        Special
    }

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f; // Vida máxima del jugador
    [SerializeField] public float currentHealth;
    [SerializeField] private float healthRecoveryRate = 1f; // Velocidad de recuperación de vida
    [SerializeField] public bool alive = true;
    [SerializeField] public bool hurted = false;

    [Header("Variables")]
    
    [SerializeField] public float movingSpeed;
    [SerializeField] private float runnningSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] private float forceMagnitude;

    [Header("Falling Damage")]
    [SerializeField] private float fallThreshold = 12.5f; // Velocidad mínima para sufrir daño al caer
    [SerializeField] private bool isFalling = false;
    [SerializeField] private float fallSpeed = 0f;
    [SerializeField] private bool hasLanded = false;

    [Header("Checking Attributes")]
    [SerializeField] private bool hasJumped = false;
    [SerializeField] private GrabDetection grabCheck;
    [SerializeField] private bool isGrounded;
    public MovementState state;
    public RotationState r_state;

    // Ledge Climbing
    [Header("Ledge Info")]
    [SerializeField] private Vector3 offset1; // Offest for position before climb
    [SerializeField] private Vector3 offset2; // Offset for position after climb
    [HideInInspector] public bool ledgeDetected;

    [Header("SFX")]
    private AudioClip currentAudioClip;
    [SerializeField] private AudioClip meowingSound;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip sprintingSound;

    [Header("Misc")]
    [SerializeField] private LayerMask collidableLayers;
    [SerializeField] public LayerMask climableLayers;
    [SerializeField] public bool hasKey = false;
    
    [SerializeField] private GameObject PopUp;
    private SpriteRenderer sprite_PopUp;
    [SerializeField] public bool showPopup = false;

    // Ground Collision Detection
    private Vector3 boxColliderCenter;
    private Vector3 boxColliderSize;

    // Camera Changes
    public float velocidadRotacion = 7.5f;
    public Quaternion rotacionInicial;
    public Quaternion rotacionObjetivo;

    // SFX Variables
    private bool canMeow = true;
    private float timerMeow = 1.5f;

    // PopUp
    private Color transparentColor = new Color(1f, 1f, 1f, 0f); // Color transparente (0% opacidad)
    private Color opaqueColor = new Color(1f, 1f, 1f, 1f); // Color opaco (100% opacidad)

    private Vector3 climbBegunPosition;
    private Vector3 climbOverPosition;
    private bool canGrabLedge = true;
    private bool canClimb;

    private void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bCollider = GetComponent<BoxCollider>();
        source = GetComponent<AudioSource>();
        sprite_PopUp = PopUp.GetComponent<SpriteRenderer>();

        boxColliderCenter = bCollider.center;
        boxColliderSize = bCollider.size;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!PauseMenuLogic.isPaused && !FrameLogic.onCinematic && !VerificarCinemática.onIntro)
        {
            animator.SetBool("isJumping", hasJumped);
            animator.SetBool("canClimb", canClimb);

            // Basic Movement
            Move();
            Jump();
            Run();

            // SFX
            SFXState();
            Meow();

            // Check for Things
            CheckForLedge();
            
            ChangeRotation();
            TimerPopUp();

            HealthRecover();
        }
    }

    private void Move()
    {
        float moveHorizontal = 0;
        float moveVertical = 0;

        if (alive)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }

        if (r_state == RotationState.Zero)
        {
            moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        }
        else if (r_state == RotationState.Ninety)
        {
            moveDirection = new Vector3(-moveVertical, 0f, moveHorizontal).normalized;
        }
        else if (r_state == RotationState.HunEighty)
        {
            moveDirection = new Vector3(-moveHorizontal, 0f, -moveVertical).normalized;
        }
        else if (r_state == RotationState.Special)
        {
            moveDirection = new Vector3(moveVertical, 0f, -moveHorizontal).normalized;
        }


        if (moveDirection.sqrMagnitude > 0.01f && r_state == RotationState.Zero)
        {
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.z);
            state = MovementState.Walking;
        }
        else if (moveDirection.sqrMagnitude > 0.01f && r_state == RotationState.Ninety)
        {
            animator.SetFloat("Horizontal", moveDirection.z);
            animator.SetFloat("Vertical", -moveDirection.x);
            state = MovementState.Walking;
        }
        else if (moveDirection.sqrMagnitude > 0.01f && r_state == RotationState.HunEighty)
        {
            animator.SetFloat("Horizontal", -moveDirection.x);
            animator.SetFloat("Vertical", -moveDirection.z);
            state = MovementState.Walking;
        }
        else if (moveDirection.sqrMagnitude > 0.01f && r_state == RotationState.Special)
        {
            animator.SetFloat("Horizontal", -moveDirection.z);
            animator.SetFloat("Vertical", moveDirection.x);
            state = MovementState.Walking;
        }
        else
        { 
            state = MovementState.Idle;
        }

        animator.SetFloat("Speed", moveDirection.sqrMagnitude);

        rb.velocity = new Vector3(moveDirection.x * movingSpeed, rb.velocity.y, moveDirection.z * movingSpeed);
    }

    private void Jump()
    {
        CheckIfGrounded();

        if (isGrounded && alive && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            hasJumped = true;
            
            state = MovementState.Air;

            Invoke("DeactiveJumpAnim", 1f);

        }
    }

    private void DeactiveJumpAnim() => hasJumped = false;

    private void Run()
    {
        if (alive && Input.GetKey(KeyCode.LeftShift) && !grabCheck.isGrabbing)
        {
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            horizontalVelocity = horizontalVelocity.normalized * runnningSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);

            animator.speed = 1.5f;
            
            state = MovementState.Sprinting;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    private void CheckIfGrounded()
    {
        Vector3 origin = transform.position + boxColliderCenter - new Vector3(0f, .05f, 0f);
        Vector3 boxCastSize = new Vector3(2.8f, 0.5f, 0.7f);

        if (Physics.CheckBox(origin, Vector3.Scale(boxColliderSize, boxCastSize), Quaternion.identity, collidableLayers))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            state = MovementState.Air;
        }
    }

    /*private void CheckForFallDamage()
    {
        if (state == MovementState.Air)
        {
            fallSpeed = -rb.velocity.y;
            isFalling = true;
        }

        // Verificar si el jugador ha aterrizado
        if (isFalling && state != MovementState.Air && !canClimb)
        {
            if (!hasLanded && fallSpeed > fallThreshold)
            {
                float damage = CalculateDamage(fallSpeed);
                TakeDamage(damage);
            }

            hasLanded = true;
        }

        // Actualizar el estado de caída
        isFalling = (state == MovementState.Air);

        // Reiniciar el indicador de aterrizaje al tocar el suelo
        if (isGrounded)
        {
            hasLanded = false;
            hasBeenHurt = false;
        }

        // Recuperación progresiva de vida
        if (!isFalling && currentHealth < maxHealth)
        {
            
            currentHealth += healthRecoveryRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    private float CalculateDamage(float fallSpeed)
    {
        // Cálculo del daño basado en la velocidad de caída
        float damage = Mathf.Floor(fallSpeed) * 5f;

        return damage;
    }*/

    private void HealthRecover()
    {
        if (currentHealth < maxHealth)
        {

            currentHealth += healthRecoveryRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        

        if (currentHealth <= 0f)
        {
            alive = false;
        }

        hurted = true;
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            fallSpeed = 0;

            canGrabLedge = false;

            Vector3 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
            
        }

        if (canClimb)
        {
            fallSpeed = 0;

            state = MovementState.Climbing;
            transform.position = climbBegunPosition;
        }
    }

    private void LedgeClimbOver()
    {
        fallSpeed = 0;
        transform.position = climbOverPosition;
        canClimb = false;
        Invoke("AllowLedgeGrab", .5f);
    }


    private void AllowLedgeGrab()
    {
        canGrabLedge = true;
        fallSpeed = 0;
        //state = MovementState.Idle;
    }

    public void ChangeRotation()
    {
        Quaternion targetRotation;
        
        if (r_state == RotationState.Zero)
        {
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, velocidadRotacion * Time.deltaTime);
        }
        else if (r_state == RotationState.Ninety)
        {
            targetRotation = Quaternion.Euler(0f, -90, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, velocidadRotacion * Time.deltaTime);
        }
        else if (r_state == RotationState.HunEighty)
        {
            targetRotation = Quaternion.Euler(0f, -180f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, velocidadRotacion * Time.deltaTime);
        }
        else if (r_state == RotationState.Special)
        {
            targetRotation = Quaternion.Euler(0f, 60f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, velocidadRotacion * Time.deltaTime);
        }
    }

    private void TimerPopUp()
    {
        if (showPopup)
        {
            PopUp.SetActive(true);
            sprite_PopUp.color = Color.Lerp(sprite_PopUp.color, opaqueColor, 7.5f * Time.deltaTime);
        }
        else
        {
            sprite_PopUp.color = Color.Lerp(sprite_PopUp.color, transparentColor, 7.5f * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + boxColliderCenter - new Vector3(0f, .05f, 0f);
        Vector3 boxCastSize = new Vector3(2.8f, 0.5f, 0.7f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(origin, Vector3.Scale(boxColliderSize, boxCastSize));

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    // SFX
    private void SFXState()
    {
        if (state == MovementState.Walking)
        {
            if (currentAudioClip != walkingSound)
            {
                PlayLastingSFX(walkingSound);
                currentAudioClip = walkingSound;
            }
        }
        else if (state == MovementState.Sprinting)
        {
            if (currentAudioClip != sprintingSound)
            {
                PlayLastingSFX(sprintingSound);
                currentAudioClip = sprintingSound;
            }
        }
        else if (state == MovementState.Idle)
        {
            if (currentAudioClip != null)
            {
                source.Stop();
                currentAudioClip = null;
            }
        }
    }

    private void PlayLastingSFX(AudioClip clip)
    {
        if (currentAudioClip != clip)
        {
            source.loop = true;
            source.Stop();
            source.clip = clip;
            source.Play();
            currentAudioClip = clip;
        }
    }

    private void Meow()
    {
        if (canMeow && Input.GetKeyDown(KeyCode.Z))
        {
            source.PlayOneShot(meowingSound);
            canMeow = false;
        }

        if (!canMeow && timerMeow >= 0)
        {
            timerMeow -= Time.deltaTime;
        }
        else
        {
            timerMeow = 1.5f;
            canMeow = true;
        }
    }
}



