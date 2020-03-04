using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region private variables
    GameObject _goal;
    GameController _gameController;
    float _rotateSpeed = 200.0f, _powerMin = 5f, _powerMax = 500f, _chargeTime = 5f, _chargeTimeCurr = 0f, _currHeight = 0f, _prevHeight = 0f, _travel = 0f,
        _horizontalAxis = 0f, _verticalAxis = 0;
    bool _hardMode = true, _isCharging = false, _spaceUp = true, _fired = false;
    Quaternion _originalRotation, _userRotation;
    Vector3 _originalPos;
    #endregion

    #region public variables
    public GameObject _directionalIndicator;
    #endregion

    #region private functions
    void Start()
    {

        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if (_gameController == null)
        {
            Debug.Log("GameController could not be found.");
        }
        _goal = GameObject.FindWithTag("Goal");
        if (_goal == null)
        {
            Debug.Log("The goal for this course could not be found.");
        }

        _originalRotation = transform.rotation; //saves initial rotation for refiring so that the ball is not at a strange angle due to rolling
        _originalPos = transform.position; //saves start inital position (ball location) for resetting
    }

    // Update is called once per frame

    void Update()
    {
        if(!_fired) //can only rotate if ball is not moving (not being fired)
        {

            //rotates the ball up/down, left/right on its axis

            _horizontalAxis += Input.GetAxisRaw("Horizontal") * _rotateSpeed * Time.deltaTime;
            _verticalAxis += Input.GetAxisRaw("Vertical") * _rotateSpeed * Time.deltaTime;

            _userRotation = Quaternion.Euler(-_verticalAxis, _horizontalAxis, 0);
            transform.rotation = _userRotation;

            //uses Quaternions and Euler angles to get absolute rotation, not local
            //makes controlling rotation easier

        }


        if (Input.GetKeyDown(KeyCode.Space) && _spaceUp && _fired == false)
        {
            if(!_hardMode) //if hard mode is not enabled then ball will reset to previous (not initial) position when OutOfBounds is hit
            {
                _originalPos = transform.position;
            }
            _chargeTimeCurr = 0f; //the longer space is pressed = more power
            _isCharging = true;
            _spaceUp = false;
            Debug.Log("Charge time: " + _chargeTimeCurr);
            
        }

        if (_isCharging && Input.GetKeyUp(KeyCode.Space) && _fired == false)
        {
            _spaceUp = true;
            float power = Mathf.Lerp(_powerMin, _powerMax, _chargeTimeCurr / _chargeTime);
            //interpolates between min power, max power over the max charge time
            Debug.Log("Power: " + power);
            _fired = true;
            _directionalIndicator.GetComponent<Renderer>().enabled = false;
            HitBall(power);
        }

        if (_isCharging)
        {
            _chargeTimeCurr += Time.deltaTime; //increases charge over time, when space is pressed
        }

        Rigidbody r = GetComponent<Rigidbody>();
        float movementSpeed = r.velocity.magnitude;

        _currHeight = transform.position.y;

        _travel = Mathf.Abs(_currHeight - _prevHeight); //magnitude does not detect falling so this is done manually

        if(_travel == 0 && movementSpeed == 0 && _fired && _spaceUp) //check the ball is not moving, can then reset
        {
            _fired = false;
            Debug.Log("!!!! STOPPED MOVING !!!!!!");
            _horizontalAxis = 0;
            _verticalAxis = 0; //set these back to 0 to stop flickering back to prev rotation
            transform.rotation = _originalRotation; //resets rotation
            _directionalIndicator.GetComponent<Renderer>().enabled = true;
        }

        _prevHeight = _currHeight;

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
