using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private Transform target;
    private Vector3 initialPostion;

    // Start is called before the first frame update
    void Start()
    {
        initialPostion = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 newPos = new Vector3(target.position.x, initialPostion.y, (target.position.z + initialPostion.z));
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed);
    }
}
