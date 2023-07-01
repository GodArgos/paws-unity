using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorLogic : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private HingeJoint hingeJoint;
    [SerializeField] private Rigidbody rb;

    private JointMotor originalMotor;
    private JointLimits originalLimits;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        hingeJoint = GetComponent<HingeJoint>();

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
                player.hasKey = false;

                rb.isKinematic = false;

                // Restablecer el comportamiento original del HingeJoint
                hingeJoint.useMotor = true;
                hingeJoint.useLimits = true;

                // Restaurar las características originales del HingeJoint
                hingeJoint.motor = originalMotor;
                hingeJoint.limits = originalLimits;

            }
            else
            {
                // Mostar Burbuja de necesitar llave
            }
        }
    }
}
