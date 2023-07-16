using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlayerController;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private PlayerController player;
    [SerializeField] private float horizonValues;
    [SerializeField] private float verticalValues;

    private Vector3 initialPosition;
    private Vector3 offset;
    private bool canDectected;

    private void Start()
    {
        initialPosition = transform.position - player.transform.position;
    }

    private void Update()
    {
        ChangeCheckerPosition();
        if(canDectected )
        {
            player.ledgeDetected = Physics.CheckSphere(transform.position, radius, player.climableLayers);
        }
        
    }

    private void ChangeCheckerPosition()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (player.r_state == RotationState.Zero)
        {
            if (horizontalInput < 0) // Left - A
            {
                offset = new Vector3(-horizonValues, 0, 0);
                player.switchToVerti = false;
            }
            else if (horizontalInput > 0) // Right - D
            {
                offset = new Vector3(horizonValues, 0, 0);
                player.switchToVerti = false;
            }
            else if (verticalInput < 0) // Front - S - Down
            {
                offset = new Vector3(0, 0, -verticalValues);
            }
            else if (verticalInput > 0) // Back - W - Up
            {
                offset = new Vector3(0, 0, verticalValues);
                player.switchToVerti = true;
            }
        }
        else if (player.r_state == RotationState.Ninety)
        {
            if (horizontalInput < 0) // Left - A
            {
                offset = new Vector3(0, 0, -horizonValues);
            }
            else if (horizontalInput > 0) // Right - D
            {
                offset = new Vector3(0, 0, horizonValues);
            }
            else if (verticalInput < 0) // Front - S - Down
            {
                offset = new Vector3(verticalValues, 0, 0);
            }
            else if (verticalInput > 0) // Back - W - Up
            {
                offset = new Vector3(-verticalValues, 0, 0);
            }
        }
        else if (player.r_state == RotationState.HunEighty)
        {
            if (horizontalInput < 0) // Left - A
            {
                offset = new Vector3(horizonValues, 0, 0);
            }
            else if (horizontalInput > 0) // Right - D
            {
                offset = new Vector3(-horizonValues, 0, 0);
            }
            else if (verticalInput < 0) // Front - S - Down
            {
                offset = new Vector3(0, 0, verticalValues);
            }
            else if (verticalInput > 0) // Back - W - Up
            {
                offset = new Vector3(0, 0, -verticalValues);
            }
        }
        else if (player.r_state == RotationState.Special)
        {
            if (horizontalInput < 0) // Left - A
            {
                offset = new Vector3(-horizonValues, 0, 0);
            }
            else if (horizontalInput > 0) // Right - D
            {
                offset = new Vector3(horizonValues, 0, 0);
            }
            else if (verticalInput < 0) // Front - S - Down
            {
                offset = new Vector3(0, 0, -verticalValues);
            }
            else if (verticalInput > 0) // Back - W - Up
            {
                offset = new Vector3(0, 0, verticalValues);
            }
        }


        transform.position = player.transform.position + offset + initialPosition;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == player.climableLayers) { }
        {
            canDectected = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == player.climableLayers) { }
        {
            canDectected = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
