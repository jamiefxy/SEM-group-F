using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    GameObject goal;
    GameController _gameController;
    public GameObject _outOfBoundsObject;
    public GameObject _directionalIndicator;
    public float rotateSpeed = 200.0f;
    public float powerMin = 5f;             
    public float powerMax = 500f;              
    public float chargeTime = 5f;
    public bool hardMode = true;
    private float _chargeTimeCurr = 0f;
    private bool _isCharging = false;       
    private bool _spaceUp = true;
    private bool _fired = false;
    private Quaternion _originalRotation;
    private Vector3 _originalPos;
    private float _currHeight;
    private float _prevHeight;
    private float _travel;
    private Quaternion _userRotation;
    private float _horizontalAxis = 0;
    private float _verticalAxis = 0;


    // Start is called before the first frame update
    void Start()
    {
  
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        if(_gameController == null)
        {
            Debug.Log("GameController could not be found.");
        }

        goal = GameObject.FindWithTag("Goal");
        if(goal == null)
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

            _horizontalAxis += Input.GetAxisRaw("Horizontal") * rotateSpeed * Time.deltaTime;
            _verticalAxis += Input.GetAxisRaw("Vertical") * rotateSpeed * Time.deltaTime;

            _userRotation = Quaternion.Euler(-_verticalAxis, _horizontalAxis, 0);
            transform.rotation = _userRotation;

            //uses Quaternions and Euler angles to get absolute rotation, not local
            //makes controlling rotation easier

        }

        if (Input.GetKeyDown(KeyCode.Space) && _spaceUp && _fired == false)
        {
            if(!hardMode) //if hard mode is not enabled then ball will reset to previous (not initial) position when OutOfBounds is hit
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
            float power = Mathf.Lerp(powerMin, powerMax, _chargeTimeCurr / chargeTime);
            //interpolates between min power, max power over the max charge time
            Debug.Log("Power: " + power);
            _directionalIndicator.GetComponent<Renderer>().enabled = false;
            _fired = true;
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

        if(_travel == 0 && movementSpeed == 0) //check the ball is not moving, can then reset
        {
            _fired = false;
            Debug.Log("!!!! STOPPED MOVING !!!!!!");
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
        if(collision.gameObject == goal.gameObject)
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
}
