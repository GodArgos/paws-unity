using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpQuestionLogic : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private OpenDoorLogic openDoorLogic;
    // Start is called before the first frame update

    private void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !openDoorLogic.opened)
        {
            playerController.showPopup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController.showPopup = false;
        }
    }


}
