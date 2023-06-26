using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 newPos = new Vector3(target.position.x, 2.5f, (target.position.z - 4.5f));
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed);
    }
}
