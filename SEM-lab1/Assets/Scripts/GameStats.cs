using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStats
{
    private static float _score;
    private static int _currentLevel = 0, _stroke = 0;

    public static float Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    public static int CurrentLevel
    {
        get{
            return _currentLevel;
        }
        set
        {
            _currentLevel = value;
        }
    }

    public static int Stroke
    {
        get{
            return _stroke;
        }
        set
        {
            _stroke = value;
        }
    }
}
