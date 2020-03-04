using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region private variables
    public Text Score;
    float _score = 0.0f, _timer = 20.0f;
    int _strokeCount = 0;
    string[] _levels;
    #endregion

    #region private functions
    void Start()
    {
        Score.text = $"Strokes: {_strokeCount}";
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        Debug.Log($"Scene Count: {sceneCount}");
        _levels = new string[sceneCount];
        for(int i = 0; i < sceneCount; i++)
        {
            _levels[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        Score.text = $"Strokes: {_strokeCount}";
    }

    void EndGame()
    {
        GameStats.CurrentLevel = 0;
        GameStats.Score = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameStats.CurrentLevel);
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

        GameStats.Score += _score;
        GameStats.Stroke += _strokeCount;
        if(GameStats.CurrentLevel >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1)
        {
            EndGame();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(++GameStats.CurrentLevel);
        }
    }
    #endregion
}
