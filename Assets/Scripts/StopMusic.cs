using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusic : MonoBehaviour
{
    private AudioSource m_AudioSource;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FrameLogic.onCinematic || VerificarCinemática.onIntro)
        {
            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Pause();
            }
        }
        else
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
    }
}
