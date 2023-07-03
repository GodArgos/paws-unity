using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private bool isFlickering = false;
    [SerializeField] private float timeDelay;

    private void Start()
    {
        targetLight = GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isFlickering && targetLight != null && gameObject.layer == LayerMask.NameToLayer("Flickering"))
        {
            StartCoroutine(FlickeringLight());
        }
    }

    private IEnumerator FlickeringLight()
    {
        isFlickering = true;
        targetLight.enabled = false;
        timeDelay = 0.1f;
        yield return new WaitForSeconds(timeDelay);

        targetLight.enabled = true;
        yield return new WaitForSeconds(timeDelay);

        targetLight.enabled = false;
        yield return new WaitForSeconds(timeDelay);

        targetLight.enabled = true;
        timeDelay = 10f;
        yield return new WaitForSeconds(timeDelay);

        isFlickering = false;
    }
}
