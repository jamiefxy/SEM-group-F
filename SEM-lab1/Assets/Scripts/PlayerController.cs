using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    GameController _gameController;
    public float rotateSpeed = 5.0f;
    public float powerMin = 5f;             
    public float powerMax = 500f;              
    public float chargeTime = 5f;     
    private float _chargeTimeCurr = 0f;
    private bool _isCharging = false;       
    private bool _spaceUp = true;
    private bool _fired = false;
    private Quaternion _originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this._originalRotation = this.transform.rotation; //saves initial rotation for refiring so that the ball is not at a strange angle due to rolling
    }

    // Update is called once per frame
    void Update()
    {
        if(!_fired) //can only rotate if ball is not moving (not being fired)
        {
            transform.Rotate(-Input.GetAxis("Vertical") * rotateSpeed, Input.GetAxis("Horizontal") * rotateSpeed, 0.0f);
            //rotates the ball up/down, left/right on its axis
        }

        if (Input.GetKeyDown(KeyCode.Space) && _spaceUp && _fired == false)
        {
            _chargeTimeCurr = 0f; //the longer space is pressed = more power
            _isCharging = true;
            Debug.Log("Charge time: " + _chargeTimeCurr);
        }

        if (_isCharging && Input.GetKeyUp(KeyCode.Space) && _fired == false)
        {
            _spaceUp = true;
            float power = Mathf.Lerp(powerMin, powerMax, _chargeTimeCurr / chargeTime);
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
            this.transform.rotation = this._originalRotation; //resets rotation
        }

        if (Input.GetKeyDown(KeyCode.R)) //FOR DEBUGGING -> Pressing R reloads the level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
