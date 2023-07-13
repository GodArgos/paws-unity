using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsLogic : MonoBehaviour
{
    private bool toMain = false;
    void Start()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadGameSceneAsync()
    {
        // Espera hasta que el jugador haya hecho clic en el botón de inicio
        while (!toMain)
        {
            yield return null;
        }

        // Carga la escena de juego de forma asíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);

        // Evita que la escena se active automáticamente al finalizar la carga
        asyncLoad.allowSceneActivation = false;

        // Espera hasta que la carga de la escena esté completa
        while (!asyncLoad.isDone)
        {
            // Verifica si la carga ha llegado al 90%
            if (asyncLoad.progress >= 0.9f)
            {
                // Activa la escena de juego
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ToMainMenu()
    {
        toMain = true;
    }
}
