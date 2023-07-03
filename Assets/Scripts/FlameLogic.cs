using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLogic : MonoBehaviour
{
    [SerializeField] private GameObject[] Flames;
    /*[SerializeField] private bool areEnable = false;
    [SerializeField] private float timeDelay;*/

    private ParticleSystem partSys;
    
    public float tiempoDeshabilitado = 4f;
    public float tiempoHabilitado = 6f;

    private bool deshabilitar = false;

    void Start()
    {
        setFlamesTo(true);
    }

    private void Update()
    {
        /* if (!areEnable && Flames != null)
         {
             StartCoroutine(FlickeringFlames());
         }*/

        if (deshabilitar)
        {
            tiempoDeshabilitado -= Time.deltaTime;

            if (tiempoDeshabilitado <= 0f)
            {
                // Volver a habilitar los objetos después de 6 segundos
                setFlamesTo(true);

                // Reiniciar el temporizador de deshabilitado
                tiempoDeshabilitado = 4f;
                deshabilitar = false;
            }
        }
        else
        {
            tiempoHabilitado -= Time.deltaTime;

            if (tiempoHabilitado <= 0f)
            {
                // Deshabilitar los objetos después de 4 segundos
                setFlamesTo(false);

                // Reiniciar el temporizador de habilitado
                tiempoHabilitado = 6f;
                deshabilitar = true;
            }
        }
    }

    /*private IEnumerator FlickeringFlames()
    {
        setFlamesTo(true);
        timeDelay = 4f;
        yield return new WaitForSeconds(timeDelay);

        setFlamesTo(false);
        timeDelay = 8f;
        yield return new WaitForSeconds(timeDelay);

        setFlamesTo(true);
        timeDelay = 4f;
        yield return new WaitForSeconds(timeDelay);

        setFlamesTo(false);
    }*/

    private void setFlamesTo(bool state)
    {
        //areEnable = state;
        foreach (GameObject flame in Flames)
        {
            //flame.SetActive(state);
            partSys = flame.GetComponent<ParticleSystem>();
            
            if (state) 
            {
                partSys.Play();
            }
            else
            {
                partSys.Stop();
            }
        }
    }
}
