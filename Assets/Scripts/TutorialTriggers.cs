using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private GameObject mainObject;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GrabDetection grabController;
    [SerializeField] private GameObject sprite;

    [Header("Variables")]
    [SerializeField] private Material dissolvingMaterial;
    [SerializeField] private float dissolveAmount;
    [SerializeField] private BehaviorOption functionState;
    [SerializeField] private enum BehaviorOption
    {
        CheckMovement,
        CheckPull,
        CheckJump,
        CheckSprint,
        CheckPickUp,
        CheckUse
    }

    [SerializeField] private float timer = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    private new Renderer renderer;
    private bool startTimer = false;

    
    private void Start()
    {
        renderer = sprite.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (startTimer)
        {
            if (timer >= 0 && dissolveAmount >= 0)
            {
                dissolveAmount -= 0.045f;
                renderer.material.SetFloat("_Dissolve", dissolveAmount);
                timer -= Time.deltaTime;
            }
            else
            {
                mainObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!startTimer && CheckForBehaviour())
            {
                startTimer = true;
                renderer.material = dissolvingMaterial;
                renderer.material.SetFloat("_Dissolve", dissolveAmount);
                source.PlayOneShot(clip);
            }
        }
    }

    private bool CheckForBehaviour()
    {
        if (functionState == BehaviorOption.CheckMovement)
        {
            return playerController.moveDirection.sqrMagnitude > 0.01f;
        }

        else if (functionState == BehaviorOption.CheckPull)
        {
            return grabController.isGrabbing;
        }

        else if (functionState == BehaviorOption.CheckJump)
        {
            return Input.GetKey(KeyCode.Space);
        }

        else if (functionState == BehaviorOption.CheckSprint)
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        else if (functionState == BehaviorOption.CheckPickUp)
        {
            return Input.GetKey(KeyCode.E) && !playerController.hasKey;
        }

        else if (functionState == BehaviorOption.CheckUse)
        {
            return Input.GetKey(KeyCode.E) && playerController.hasKey;
        }
        
        return false;
        
    }
}
