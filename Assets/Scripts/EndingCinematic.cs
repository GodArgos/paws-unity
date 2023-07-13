using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EndingCinematic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cinematic;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GameObject music;
    public static bool isEnding = false;
    
    void Start()
    {
        cinematic.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (director.state == PlayState.Playing)
        {
            isEnding = true;
            music.SetActive(false);
        }
        else
        {
            isEnding = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cinematic.SetActive(true);
            director.Play();
        }
    }
}
