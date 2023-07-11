using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FrameLogic : MonoBehaviour
{
    private new Renderer renderer;
    private Coroutine rippleCoroutine;
    [SerializeField] private float rippleTime = 1.5f;
    [SerializeField] private float maxRippleStrength = 0.75f;

    private void Start()
    { 
        renderer = GetComponent<Renderer>();
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
}
