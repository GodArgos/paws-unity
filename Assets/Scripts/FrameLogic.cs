using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class FrameLogic : MonoBehaviour
{
    private new Renderer renderer;
    private Coroutine rippleCoroutine;
    [SerializeField] private float rippleTime = 1.5f;
    [SerializeField] private float maxRippleStrength = 0.75f;
    [SerializeField] private GameObject video;
    [SerializeField] private float timeToStop;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject FX;
    [SerializeField] private GameObject tPoint;

    [SerializeField] private GameObject VolumeEffects;

    private bool hasStarted = false;
    private int count = 0;
    private float timer = 22f;
    private BoxCollider bcollider;

    public static bool onCinematic = false;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        bcollider = GetComponent<BoxCollider>();
        video.SetActive(false);
    }

    private void Update()
    {
        if (hasStarted) 
        { 
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                VolumeEffects.SetActive(true);
                hasStarted = false;
                playerController.transform.position = tPoint.transform.position;
                onCinematic = false;
                bcollider.isTrigger = false;
            }
        } 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var mat = renderer.material;
            mat.SetVector("_Ripple_Center", other.transform.position);
            mat.SetFloat("_Ripple_Count", 2f);
            mat.SetFloat("_Ripple_Speed", 3.5f);

            if (rippleCoroutine != null)
            {
                StopCoroutine(rippleCoroutine);
            }

            rippleCoroutine = StartCoroutine(DoRipple(mat));

            count++;

            if (count == 1)
            {  
                PlayVideo();
            }
        }
    }

    private IEnumerator DoRipple(Material mat)
    {
        for (float i = 0.0f; i < rippleTime; i += Time.deltaTime)
        {
            mat.SetFloat("_Ripple_Strength", maxRippleStrength * (1.0f - i / rippleTime));
            yield return null;
        }
    }

    private void PlayVideo()
    {
        VolumeEffects.SetActive(false);
        onCinematic = true;
        Destroy(FX);
        video.SetActive(true);
        hasStarted = true;
        Destroy(video, timeToStop);
    }
}
