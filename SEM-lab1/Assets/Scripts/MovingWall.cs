using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{

#region public variables
    public Vector3 start, end; // Start and End vector coordinates for linear movement path
    public bool oscillate; // Boolean setting to decide whether movement pattern resets or reverses
    public float seconds; // Setting for how long the movement should take to complete
#endregion

#region private variables
    Vector3 _difference, _current;
    float _timer, _fraction;
#endregion

#region private functions
    void Start()
    {
        _difference = start - end;
        _current = (transform.position - end);
        var xDiff = _current.x / _difference.x;
        var yDiff = _current.y / _difference.y;
        var zDiff = _current.z / _difference.z;
        _fraction = xDiff > 0 ? xDiff : yDiff > 0 ? yDiff : zDiff > 0 ? zDiff : 0;
        _timer = _fraction*seconds;
    }

    void Update()
    {
        if (_timer <= seconds) {
            _timer += Time.deltaTime;
            transform.position = start - _difference * (_timer / seconds);
        }
        else
        {
            if(oscillate)
            {
                var temp = start;
                start = end;
                end = temp;
                _difference = start - end;
            }
            else
            {
                transform.position = start;
            }
            _timer = 0;
        }
    }
}
#endregion