using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudLogic : MonoBehaviour
{
    [SerializeField] private GameObject HUD;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;
    [SerializeField] private FrameLogic frameLogic;

    void Start()
    {
        HUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!FrameLogic.onCinematic)
        {
            if (playerController.hasKey)
            {
                HUD.SetActive(true);
                animator.SetBool("hasBeenUsed", false);
            }
        }
        else
        {
            HUD.SetActive(false);
        }
        
    }
}
