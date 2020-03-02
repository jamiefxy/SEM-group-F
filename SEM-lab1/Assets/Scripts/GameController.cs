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
    string[] _levels;
    int _currentLevel = 0;
    #endregion

    #region private functions
    void Start()
    {
        Score.text = $"Score: {_score}\nTimer: {_timer}\nStroke: {_strokeCount}";
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        _levels = new string[sceneCount];
        for(int i = 0; i < sceneCount; i++)
        {
            _levels[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }
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

    void EndGame()
    {
        _score = 0;
        _timer = 0.0f;
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
        // Log current score and start the next level
        // If the final level is completed, call End()
    }
    #endregion
}
