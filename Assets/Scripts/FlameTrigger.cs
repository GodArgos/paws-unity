using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlameTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Flames;
    [SerializeField] private GameObject Tutorial;
    private int count = 0;
    void Start()
    {
        Flames.SetActive(false);
        Tutorial.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (count == 0)
            {
                count++;
                Flames.SetActive(true);
                Tutorial.SetActive(true);
            }
        }
    }
}
