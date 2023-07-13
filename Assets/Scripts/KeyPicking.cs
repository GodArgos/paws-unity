using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPicking : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animHUD;
    [SerializeField] private GameObject HUD;
    [SerializeField] private AudioClip KeySFX;
    private AudioSource source;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
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

                count += 1;

                if (count == 1)
                {
                    source.PlayOneShot(KeySFX);
                }
            }
        }
    }

    private void Disappear()
    {
        
        player.hasKey = true;
        this.gameObject.SetActive(false);
    }
}
