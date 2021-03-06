﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject _player;
    Vector3 _offset;
    float _smoothSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            Debug.Log("Player not Found.");
        }
        _offset = new Vector3(0.0f,10.0f,-12.0f);
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = _player.transform.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;
    }
}
