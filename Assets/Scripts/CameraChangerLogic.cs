using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangerLogic : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerController.RotationState initial_rotationState;
    [SerializeField] private PlayerController.RotationState target_rotationState;


    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag("Player"))
        {
            playerController.r_state = target_rotationState;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.r_state = initial_rotationState;
        }
    }
}
