using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region private variables
    public Text Score;
    float _score = 0.0f, _timer = 5.0f;
    int _strokeCount = 0;
    #endregion

    #region private function
    void Start()
    {
        Score.text = $"Score: {_score}\nTimer: {_timer}\nStroke: {_strokeCount}";
    }

    void Update()
    {
        
        _timer -= Time.deltaTime;
        Score.text = $"Score: {_score}\nTimer: {(int)_timer}\nStroke: {_strokeCount}";
    }

    // Reset the stroke count to 0
    void ResetStrokeCount()
    {
        _strokeCount = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        _score += 1;
        _timer = 6.0f;
    }
    #endregion

    #region public functions
    // Increase stroke count by 1
    public void IncrementStrokeCount()
    {
        _strokeCount++;
    }

    public void GoalReached()
    {
        // stub
    }
    #endregion
}
