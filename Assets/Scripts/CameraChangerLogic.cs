using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangerLogic : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private string targetTag;
    [SerializeField] private CinemachineVirtualCamera targetCamera;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            playerController.viewChanged = true;
            targetCamera.Priority = 11;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            playerController.viewChanged = false;
            targetCamera.Priority = 9;
        }
    }
}
