using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPicking : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                animator.SetBool("toDisappear", true);
            }
        }
    }

    private void Disappear()
    {
        player.hasKey = true;
        this.gameObject.SetActive(false);
    }
}
