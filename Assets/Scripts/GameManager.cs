using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameover;

    public bool _iscoOpMode = false;
    private int _playersAlive = 1;
    [SerializeField]
    private UI_Manager _uiManager;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    [SerializeField]
    private Animator _pauseAnimator;
    public void Start()
    {

        


        _playersAlive = _iscoOpMode ? 2 : 1;

        if (_uiManager == null)
        {
            _uiManager = FindObjectOfType<UI_Manager>();
        }
       



    }

    public void Update()
    {
        
        //if the key r was pressed
        //restart the current scene
        if (Input.GetKeyDown(KeyCode.R) && _isGameover==true)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseMenuPanel.gameObject.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            Time.timeScale = 0;
            
        }
    }
    
    
    public void Gameover()
    {
        if (_isGameover)
            return;
        _isGameover = true;

        if (_uiManager != null)
        {
            _uiManager.ShowGameOverUI();
        }
        Spawn_Manager spawnManager = FindObjectOfType<Spawn_Manager>();
        if (spawnManager != null)
        {
            spawnManager.StopSpawning();
        }
    }

    public void OnPlayerDeath()
    {
        _playersAlive--;
        if (_iscoOpMode == false)
        {
            Gameover();
        }
        else
        {
            if (_playersAlive <= 0)
            {
                Gameover();
            }
        }
    }

    public void pauseMenuPanelDisable()
    {
        _pauseMenuPanel.gameObject.SetActive(false);

    }
}
