using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameController _gameController;
    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Debug.Log("Input: " + horizontalMovement + " " + verticalMovement);

        Rigidbody r = GetComponent<Rigidbody>();
        r.velocity = new Vector3(horizontalMovement * speed, 0.0f, verticalMovement * speed);

    }

    // Increment the stored stroke count when the ball is hit by the player
    void HitBall()
    {
        _gameController.IncrementStrokeCount();
    }
}
