using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region private variables
    float _score = 0.0f, _timer = 5.0f;
    int _strokeCount = 0;
    bool playing = false;
    PlayerController playerController;
    #endregion

    #region public variables
    public Text Score;
    public Text Info;
    public GameObject menu;
    public GameObject Helpmenu;
    public GameObject Startmenu;
    public GameObject directionalIndicator;
    public Button start;
    public Button help;
    public Button back;
    #endregion

    #region private functions
    void Start()
    {
        Score.text = $"Score: {_score}\nTimer: {_timer}\nStroke: {_strokeCount}";

        start.onClick.AddListener(StartGame);
        help.onClick.AddListener(Help);
        back.onClick.AddListener(Back);

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        Score.enabled = false;
        menu.SetActive(true);
        directionalIndicator.SetActive(false);
        playerController.enabled = false;
    }

    void Update()
    {
        if(playing)
        {
            _timer -= Time.deltaTime;
            Score.text = $"Score: {_score}\nTimer: {(int)_timer}\nStroke: {_strokeCount}";
        }
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

    public void StartGame()
    {
        
        Score.enabled = true; 
        menu.SetActive(false); 
        directionalIndicator.SetActive(true); 
        playerController.enabled = true; 

        // Hide the Menu UI and re-enable the PlayerController script, the Directional Indicator and the Score text field
    }

    public void Help() 
    {

        Startmenu.SetActive(false);
        Helpmenu.SetActive(true);
        Info.enabled = true;   

       // Info.text = $"\tInstructions\n\n Use the L and R Keys to aim for the Hole.\n Press down space and release to launch the golf ball.";   
    }

    public void Back() 
    {
        Startmenu.SetActive(true);
        Helpmenu.SetActive(false);
        Info.enabled = false; 
    }
    
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
