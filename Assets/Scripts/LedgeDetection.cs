using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private PlayerController player;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float horizonValues;
    [SerializeField] private float verticalValues;

    private Vector3 initialPosition;
    private Vector3 offset;

    private void Start()
    {
        initialPosition = transform.position - player.transform.position;
    }

    private void Update()
    {
        ChangeCheckerPosition();
        player.ledgeDetected = Physics.CheckSphere(transform.position, radius, layerMask);
    }

    void ChangeCheckerPosition()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

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

        transform.position = player.transform.position + offset + initialPosition;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
