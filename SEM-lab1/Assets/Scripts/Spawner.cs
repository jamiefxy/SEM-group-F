using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prototype;
    // Start is called before the first frame update
    void Start()
    {

        //Vector3 setUpPosition = Vector3.Zero; 
        //Transform t = this.GetComponent<Transform>();
        //t.position = setUpPosition;
        Vector3 randomPosition = new Vector3(Random.Range(-4.0f, 3.61f), (0.48f), Random.Range(-4.0f, 3.61f));
        GameObject spawned = Instantiate(prototype, randomPosition, Quaternion.identity);
    }

    void OnCollisionEnter(Collision other)
    { // AT every collision create a new spawned object at zero – useless again so don’t just copy this.
        Vector3 randomPosition = new Vector3(Random.Range(-4.0f, 3.61f), (0.48f), Random.Range(-4.0f, 3.61f));
        GameObject spawned = Instantiate(prototype, randomPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
