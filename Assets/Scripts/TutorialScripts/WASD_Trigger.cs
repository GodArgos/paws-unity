using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject sprite;
    [SerializeField] private Material dissolvingMaterial;
    [SerializeField] private float dissolveAmount;

    private Renderer renderer;

    private void Start()
    {
        renderer = sprite.GetComponent<Renderer>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            /*if (playerController.moveDirection.sqrMagnitude > 0.01f)
            {
                
            }*/

            renderer.material = dissolvingMaterial;
            renderer.material.SetFloat("_Dissolve", dissolveAmount);

        }
    }
}
