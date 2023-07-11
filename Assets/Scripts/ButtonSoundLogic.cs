using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundLogic : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip highlightClip;
    [SerializeField] private AudioClip pressClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnHighlight()
    {
        audioSource.PlayOneShot(highlightClip);
    }

    private void OnPressed()
    {
        audioSource.PlayOneShot(pressClip);
    }
}
