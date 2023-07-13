using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLogic : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject[] Flames;
    private AudioSource source;

    private ParticleSystem partSys;

    public float tiempoDeshabilitado = 4f;
    public float tiempoHabilitado = 6f;
    private bool deshabilitar = false;

    public static bool isDamagable = false;
    private float fireDamage = 0f;

    [SerializeField] private float cooldown = 2f;

    void Start()
    {
        source = GetComponent<AudioSource>();
        setFlamesTo(true);
        source.loop = true;
    }

    private void Update()
    {
        if (deshabilitar)
        {
            fireDamage = 0f;
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
            cooldown -= Time.deltaTime;

            if (isDamagable && cooldown <= 0)
            {
                fireDamage = 70f;
                playerController.TakeDamage(fireDamage);
                cooldown = 2f;
            }
            

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

    private void setFlamesTo(bool state)
    {
        foreach (GameObject flame in Flames)
        {
            partSys = flame.GetComponent<ParticleSystem>();
            
            if (state) 
            {
                if (!source.isPlaying)
                {
                    source.Play();
                }
                partSys.Play();
            }
            else
            {
                if (source.isPlaying)
                {
                    source.Pause();
                }
                partSys.Stop();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isDamagable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isDamagable = false;
        }
    }
}
