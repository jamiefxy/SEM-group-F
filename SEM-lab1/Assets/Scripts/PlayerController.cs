using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameController gameController;
    GameObject goal;

    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if(gameController == null)
        {
            Debug.Log("GameController could not be found.");
        }

        goal = GameObject.FindWithTag("Goal");
        if(goal == null)
        {
            Debug.Log("The goal for this course could not be found.");
        }
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

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject == goal.gameObject)
        {
            gameController.GoalReached();
        }
    }

    // Increment the stored stroke count when the ball is hit by the player
    void HitBall()
    {
        _gameController.IncrementStrokeCount();
    }
}
