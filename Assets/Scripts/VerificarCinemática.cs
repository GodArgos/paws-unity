using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VerificarCinemática : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private GameObject music;
    public static bool onIntro = false;

    // Update is called once per frame
    private void Start()
    {
        music.SetActive(false);
    }

    void Update()
    {
        if (playableDirector.state == PlayState.Playing)
        {
            onIntro = true;
        }
        else
        {
            onIntro = false;
            music.SetActive(true);
        }
    }
}
