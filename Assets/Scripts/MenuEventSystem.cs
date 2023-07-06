using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEventSystem : MonoBehaviour
{
    [HideInInspector] public bool startGame = false;

    private void Start()
    {
        StartCoroutine(LoadGameSceneAsync());
    }

    IEnumerator LoadGameSceneAsync()
    {
        // Espera hasta que el jugador haya hecho clic en el botón de inicio
        while (!startGame)
        {
            yield return null;
        }

        // Carga la escena de juego de forma asíncrona
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

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

    public void QuitGame()
    {
        Debug.Log("Quit Pressed.");
        Application.Quit();
    }
}
