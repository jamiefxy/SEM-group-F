using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region private variables
    GameObject _goal;
    GameController _gameController;
    float _rotateSpeed = 5f, _powerMin = 5f, _powerMax = 500f, _chargeTime = 5f, _chargeTimeCurr = 0f;
    bool _hardMode = true, _isCharging = false, _spaceUp = true, _fired = false;
    Quaternion _originalRotation;
    Vector3 _originalPos;
    #endregion

    #region public variables
    public GameObject _directionalIndicator;
    #endregion

    #region private functions
    void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if(_gameController == null)
        {
            Debug.Log("GameController could not be found.");
        }
        _goal = GameObject.FindWithTag("Goal");
        if(_goal == null)
        {
            Debug.Log("The goal for this course could not be found.");
        }
        _originalRotation = transform.rotation; //saves initial rotation for refiring so that the ball is not at a strange angle due to rolling
    }

    void Awake()
    {
        _originalPos = transform.position; //saves start inital position (ball location) for resetting
    }

    void Update()
    {
        if(!_fired) //can only rotate if ball is not moving (not being fired)
        {
            transform.Rotate(-Input.GetAxis("Vertical") * _rotateSpeed, Input.GetAxis("Horizontal") * _rotateSpeed, 0.0f);
            //rotates the ball up/down, left/right on its axis
        }

        if (Input.GetKeyDown(KeyCode.Space) && _spaceUp && _fired == false)
        {
            if(!_hardMode) //if hard mode is not enabled then ball will reset to previous (not initial) position when OutOfBounds is hit
            {
                _originalPos = transform.position;
            }
            _chargeTimeCurr = 0f; //the longer space is pressed = more power
            _isCharging = true;
            Debug.Log("Charge time: " + _chargeTimeCurr);
        }

        if (_isCharging && Input.GetKeyUp(KeyCode.Space) && _fired == false)
        {
            _spaceUp = true;
            float power = Mathf.Lerp(_powerMin, _powerMax, _chargeTimeCurr / _chargeTime);
            //interpolates between min power, max power over the max charge time
            Debug.Log("Power: " + power);
            _fired = true;
            HitBall(power);
        }

        if (_isCharging)
        {
            _chargeTimeCurr += Time.deltaTime; //increases charge over time, when space is pressed
        }

        Rigidbody r = GetComponent<Rigidbody>();
        float movementSpeed = r.velocity.magnitude;

        if (movementSpeed < 0.05 && _fired) //checks if the ball is still moving, ie has a magnitude
        {
            _fired = false;
            Debug.Log("!!!! STOPPED MOVING !!!!!!");
            transform.rotation = _originalRotation; //resets rotation
        }

        if (Input.GetKeyDown(KeyCode.R)) //FOR DEBUGGING -> Pressing R reloads the level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject == _goal.gameObject)
        {
            _gameController.GoalReached();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision");
        if (col.gameObject.tag == "OutOfBounds") //any object can have OutOfBounds tag, which will reset ball position
        {
            Debug.Log("Out of bounds");
            Rigidbody r = GetComponent<Rigidbody>();
            r.constraints = RigidbodyConstraints.FreezeAll; //stops the previous force from being applied to object when respawning
            transform.rotation = _originalRotation; //resets rotation and position
            transform.position = _originalPos;
            r.constraints = RigidbodyConstraints.None; //object can now have force applied again
            //only resetting the ball postion so other variables such as _strokeCount are maintained
        }
    }

    // Increment the stored stroke count when the ball is hit by the player
    void HitBall(float power)
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.AddForce(r.transform.forward * power, ForceMode.Impulse); //adds force (using power value) in the direction the ball is 'facing'
        _gameController.IncrementStrokeCount();
    }
    #endregion
}
