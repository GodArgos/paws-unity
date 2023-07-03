using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangerLogic : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CinemachineVirtualCamera targetCamera;
    [SerializeField] private bool changeCamera;
    [SerializeField] private string targetTag;

    [SerializeField] public Vector3 initialRotation;
    [SerializeField] public Vector3 targetRotation;

    private int initialPriority;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.CompareTag(targetTag))
        {
            playerController.rotacionObjetivo = Quaternion.Euler(targetRotation);
            playerController.viewChanged = true;

            if (changeCamera)
            {
                initialPriority = targetCamera.Priority;
                targetCamera.Priority = 11;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            playerController.rotacionInicial = Quaternion.Euler(initialRotation);
            playerController.viewChanged = false;

            if (targetCamera)
            {
                targetCamera.Priority = initialPriority;
            }
        }
    }
}
