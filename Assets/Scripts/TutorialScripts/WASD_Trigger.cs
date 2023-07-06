using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController.moveDirection.sqrMagnitude > 0.01f)
            {
                
            }
        }
    }
}
