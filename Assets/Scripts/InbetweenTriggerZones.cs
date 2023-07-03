using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InbetweenTriggerZones : MonoBehaviour
{
    [SerializeField] private CameraChangerLogic changerLogic;
    [SerializeField] private string targetTag;
    [SerializeField] private Vector3 initialRotationChange;
    [SerializeField] private Vector3 targetRotationChange;

    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            changerLogic.initialRotation = initialRotationChange;
        }
    }
}
