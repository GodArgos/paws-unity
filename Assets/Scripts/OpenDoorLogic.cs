using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorLogic : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private new HingeJoint hingeJoint;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject HUD;
    [SerializeField] private Animator animHUD;
    [SerializeField] public bool opened = false;
    [SerializeField] private AudioClip UnlockSFX;
    private AudioSource source;

    private JointMotor originalMotor;
    private JointLimits originalLimits;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        hingeJoint = GetComponent<HingeJoint>();
        source = GetComponent<AudioSource>();

        // Guardar las características originales del HingeJoint
        originalMotor = hingeJoint.motor;
        originalLimits = hingeJoint.limits;

        hingeJoint.useMotor = false;
        hingeJoint.useLimits = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E) && player.hasKey == true)
            {

                StartCoroutine(WaitForSound());

                player.hasKey = false;

                rb.isKinematic = false;

                // Restablecer el comportamiento original del HingeJoint
                hingeJoint.useMotor = true;
                hingeJoint.useLimits = true;

                // Restaurar las características originales del HingeJoint
                hingeJoint.motor = originalMotor;
                hingeJoint.limits = originalLimits;

                animHUD.SetBool("hasBeenUsed", true);
                //HUD.SetActive(false);

                opened = true;
                player.showPopup = false;

                
            }
        }
    }

    private IEnumerator WaitForSound()
    {
        source.PlayOneShot(UnlockSFX);
        yield return new WaitWhile(() => source.isPlaying);
    }
}
