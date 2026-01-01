using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    //handle to Text
    [SerializeField]
    private TextMeshProUGUI _scoreText,bestText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private TextMeshProUGUI _gameover_text;
    [SerializeField]
    private TextMeshProUGUI _restartText;
    
    public int playerScore,bestScore;
    

    

    
    
    
    private GameManager _gameManager;
    void Start()
    {
        bestScore = PlayerPrefs.GetInt("HighScore",0);
        bestText.text = "Highscore : " + bestScore.ToString();
        //assign text component to the handle
        _scoreText.text = "Score:" + 0;
        _gameover_text.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
       
        
        if (_gameManager == null)
        {
            Debug.LogError("_gamemanager empty");
        }
;    }

    // Update is called once per frame
    void Update()
    {
        
    } 

    public void UpdateScore()
    {
        playerScore += 10;
        _scoreText.text = "Score : " + playerScore.ToString();
    }

    //check for best score
    //if current score > best score
    //best score=current score

    public void CheckForBestScore()
    {
        if (playerScore > bestScore)
        {
            bestScore = playerScore;
            PlayerPrefs.SetInt("HighScore", bestScore);
            bestText.text = "Highscore : " + bestScore.ToString();
        }
    }
    public void UpdateLives(int currentLives)
    {
        //display img sprite
        //give it a new one based on currentlives index
        int clampedLives = Mathf.Clamp(currentLives, 0, _livesSprites.Length - 1);
        _LivesImg.sprite = _livesSprites[clampedLives];

        if(currentLives == 0 && _gameManager._iscoOpMode == false)
        {
            _gameManager.Gameover();
        }
    }

    

    IEnumerator GameoverFlickerRoutine()
    {
        while (true)
        {
            _gameover_text.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameover_text.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowGameOverUI()
    {
        _gameover_text.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameoverFlickerRoutine());
    }

    public void ResumeButton()
    {
        
        Time.timeScale = 1;
        _gameManager.pauseMenuPanelDisable();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitgameButton()
    {
        Application.Quit();
    }
    


}
