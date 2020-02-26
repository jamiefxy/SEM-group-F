using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text Score;
    public float score = 0.0f;
    public float timer = 5.0f;
    private int _strokeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        Score.text = $"Score: {score}\nTimer: {timer}\nStroke: {_strokeCount}";
    }

    // Update is called once per frame
    void Update()
    {
        
        timer -= Time.deltaTime;
        Score.text = $"Score: {score}\nTimer: {(int)timer}\nStroke: {_strokeCount}";
    }

    // Increase stroke count by 1
    public void IncrementStrokeCount()
    {
        _strokeCount++;
    }

    // Reset the stroke count to 0
    void ResetStrokeCount()
    {
        _strokeCount = 0;
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
