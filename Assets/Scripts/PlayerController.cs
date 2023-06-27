using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private BoxCollider bCollider;

    [HideInInspector] public Vector3 moveDirection;

    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Air
    }

    [Header("Variables")]
    
    [SerializeField] private float movingSpeed;
    [SerializeField] private float runnningSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float forceMagnitude;

    [Header("Checking Attributes")]
    [SerializeField] private bool isGrounded;
    public MovementState state;

    [Header("Misc")]
    [SerializeField] private LayerMask collidableLayers;
    [SerializeField] public LayerMask climableLayers;

    // Ground Collision Detection
    private Vector3 boxColliderCenter;
    private Vector3 boxColliderSize;

    // Ledge Climbing
    [Header("Ledge Info")]
    [SerializeField] private Vector3 offset1; // Offest for position before climb
    [SerializeField] private Vector3 offset2; // Offset for position after climb
    [HideInInspector] public bool ledgeDetected;

    private Vector3 climbBegunPosition;
    private Vector3 climbOverPosition;
    private bool canGrabLedge = true;
    private bool canClimb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        bCollider = GetComponent<BoxCollider>();

        boxColliderCenter = bCollider.center;
        boxColliderSize = bCollider.size;
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        Jump();
        Run();
        CheckForLedge();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;


        if (moveDirection.sqrMagnitude > 0.01f)
        {
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.z);
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

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z).normalized;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            state = MovementState.Air;

        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            horizontalVelocity = horizontalVelocity.normalized * runnningSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            
            state = MovementState.Sprinting;
        }
    }

    private void CheckIfGrounded()
    {
        Vector3 origin = transform.position + boxColliderCenter - new Vector3(0f, .05f, 0f);
        Vector3 boxCastSize = new Vector3(2.8f, 0.5f, 0.7f);

        if (Physics.CheckBox(origin, Vector3.Scale(boxColliderSize, boxCastSize), Quaternion.identity, collidableLayers))
        {
            Debug.Log("HIT");
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            state = MovementState.Air;
        }
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector3 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }

        if (canClimb)
        {
            transform.position = climbBegunPosition;
            LedgeClimbOver();
        }
    }

    private void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;

    private void Grab()
    {
        RaycastHit hit;
        
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + boxColliderCenter - new Vector3(0f, .05f, 0f);
        Vector3 boxCastSize = new Vector3(2.8f, 0.5f, 0.7f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(origin, Vector3.Scale(boxColliderSize, boxCastSize));
    }

}

