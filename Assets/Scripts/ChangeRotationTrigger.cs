using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotationTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerController.RotationState rotationState;
    
    
    [SerializeField] private Vector3 targetRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.r_state = rotationState;
        }
    }
}
