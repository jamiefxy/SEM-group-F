using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text Score;
    public float score = 0.0f;
    public float timer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Score.text = "Score: " + score + "\nTimer: " + timer;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        timer -= Time.deltaTime;
        Score.text = "Score: " + score + "\nTimer: " + (int)timer;

    }

    void OnTriggerEnter(Collider other)
    {
        score += 1;
        timer = 6.0f;
    }

    public void GoalReached()
    {
        // stub
    }
}
