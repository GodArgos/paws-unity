using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioClip hurtedSFX;

    private AudioSource source;
    private float coolDown = 2f;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
        }
       
        if (playerController.hurted && coolDown <= 0f)
        {
            playerController.hurted = false;
            source.PlayOneShot(hurtedSFX);
            coolDown = 2f;
        }
    }
}
