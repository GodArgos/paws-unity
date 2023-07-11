using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotationTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerController.RotationState rotationState;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.r_state = rotationState;
        }
    }
}
