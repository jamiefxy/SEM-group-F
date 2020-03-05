using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region private variables
    float _score = 0.0f, _timer = 20.0f;
    int _strokeCount = 0;
    string[] _levels;
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
        Score.text = $"Strokes: {_strokeCount}";
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        Debug.Log($"Scene Count: {sceneCount}");
        _levels = new string[sceneCount];
        for(int i = 0; i < sceneCount; i++)
        {
            _levels[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }

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
        Score.text = $"Strokes: {_strokeCount}";
        }
    }

    void EndGame()
    {
        GameStats.CurrentLevel = 0;
        GameStats.Score = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameStats.CurrentLevel);
    }
    #endregion

    #region public functions
    void StartGame()
    {
        Debug.Log("Start pressed");
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
