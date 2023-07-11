using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPicking : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animHUD;
    [SerializeField] private GameObject HUD;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HUD.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                HUD.SetActive(true);
                animator.SetBool("toDisappear", true);
                animHUD.SetBool("hasBeenUsed", false);
            }
        }
    }

    private void Disappear()
    {
        player.hasKey = true;
        this.gameObject.SetActive(false);
    }
}
