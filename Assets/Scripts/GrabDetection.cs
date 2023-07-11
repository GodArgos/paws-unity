using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.Tilemaps;
using UnityEngine;

public class GrabDetection : MonoBehaviour
{
    // Grabbing Mechanics
    [Header("Grab Mechanics")]
    [SerializeField] private LayerMask movableLayers;
    [SerializeField] private float radius;
    [SerializeField] private PlayerController player;
    [SerializeField] private float horizonValues;
    [SerializeField] private float verticalValues;

    [Header("SFX")]
    private AudioSource source;
    [SerializeField] AudioClip clip;

    private GameObject grabbedObject;
    public bool isGrabbing = false;
    private Vector3 grabOffset;

    private Vector3 offset;
    private Vector3 direction;
    private float distance;
    private float initialJump;
    private float initialSpeed;
    private Vector3 initialPosition;
    private float horizontalInput;
    private float verticalInput;

    private GameObject objectToGrab;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        
        initialJump = player.jumpForce;
        initialSpeed = player.movingSpeed;
        initialPosition = transform.position - player.transform.position;
    }

    private void Update()
    {
        ChangeCheckerPosition();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, movableLayers);
        if (hitColliders.Length > 0)
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag("Movable"))
                {
                    objectToGrab = collider.gameObject;
                }
            }
        }
        

        if (objectToGrab != null)
        {
            Grab(objectToGrab);
        }
    }

    private void Grab(GameObject gObject)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject == null && !isGrabbing && player.state != PlayerController.MovementState.Air)
            {
                player.jumpForce = 0f;
                player.movingSpeed = 1.5f;
                grabbedObject = gObject;
                grabOffset = grabbedObject.transform.position - player.transform.position;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                isGrabbing = true;

                source.loop = true;
                source.Play();
            }
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            if (grabbedObject != null && isGrabbing)
            {
                DeactivateGrab();
            }
        }

        if (isGrabbing && grabbedObject != null)
        {

            Vector3 newPosition = player.transform.position + grabOffset;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, distance, movableLayers) && hit.transform.CompareTag("Movable"))
            {
                DeactivateGrab();
            }
            else
            {
                grabbedObject.GetComponent<Rigidbody>().MovePosition(newPosition);
            }
        }
    }

    private void DeactivateGrab()
    {
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject = null;
        isGrabbing = false;
        player.jumpForce = initialJump;
        player.movingSpeed = initialSpeed;

        source.loop = false;
        source.Stop();
        objectToGrab = null;
    }

    private void ChangeCheckerPosition()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput < 0) // Left - A
        {
            offset = new Vector3(-horizonValues, 0, 0);
            direction = Vector3.left;
            distance = 0.7f;
        }
        else if (horizontalInput > 0) // Right - D
        {
            offset = new Vector3(horizonValues, 0, 0);
            direction = Vector3.right;
            distance = 0.7f;
        }
        else if (verticalInput < 0) // Front - S - Down
        {
            offset = new Vector3(0, 0, -verticalValues);
            direction = Vector3.back;
            distance = 0.3f;
        }
        else if (verticalInput > 0) // Back - W - Up
        {
            offset = new Vector3(0, 0, verticalValues);
            direction = Vector3.forward;
            distance = 0.3f;
        }

        transform.position = player.transform.position + offset + initialPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, direction * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
