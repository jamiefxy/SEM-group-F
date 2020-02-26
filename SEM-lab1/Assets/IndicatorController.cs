using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed = 200.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    //float axis1 = 0;
    //float axis2 = 0;
    void Update()
    {
/*        axis1 += Input.GetAxisRaw("Horizontal") * rotateSpeed * Time.deltaTime;
        axis2 += Input.GetAxisRaw("Vertical") * rotateSpeed * Time.deltaTime;

        Quaternion rotrot = Quaternion.Euler(-axis2, axis1, 0);
        transform.rotation = Quaternion.Euler(-axis2, axis1, 0);*/
    }
}
