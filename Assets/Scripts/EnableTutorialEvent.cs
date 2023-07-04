using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTutorialEvent : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private float timeRemaining;

    private void Start()
    {
        tutorialCanvas.SetActive(false);
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            tutorialCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialCanvas.SetActive(false);
        }
    }
}
