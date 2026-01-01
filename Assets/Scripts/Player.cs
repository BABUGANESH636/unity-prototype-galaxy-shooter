using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedmultiplier = 2;
    
    [SerializeField]
    private GameObject _LaserPrefab;
    
    [SerializeField]
    private GameObject _TripleshotPrefab;
    [SerializeField]
    private GameObject _ShieldPrefab;
    
    [SerializeField]
    private float _firerate = 0.15f;
    [SerializeField]
    private float _canfire = -1;
    
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _damageCooldown = 0.2f; // 0.2 sec window where extra hits are ignored
    private float _lastDamageTime = -999f;
    
    private int _score;

    public bool _isPlayer1=false;
    public bool _isPlayer2 = false;

    
    
    
    [SerializeField]
    private GameObject[] _playerHurt;
    
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    private GameManager _gameManager;
    private Spawn_Manager _spanwManager;
    private UI_Manager _uiManager;

    [SerializeField]
    private Joystick _joystick;
    private bool _fireButtonPressed = false;




    //variable is tripleshotactive 
    [SerializeField]
    private bool _isTripleshotActive = false;
    private bool _isSpeedboostActive = false;
    private bool _isShieldActive = false;
    
    [SerializeField]
    private GameObject _shieldVisualiser;
    private Animator _anim;
    private float _currentHorizontalInput;



    void Start()
    {

        _anim = GetComponent<Animator>();
        _spanwManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (_gameManager._iscoOpMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }



        if (_audioSource == null)
        {
            Debug.LogError("Audio Source Is Null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        for (int i = 1; i < _playerHurt.Length; i++)
        {
            _playerHurt[i].gameObject.SetActive(false);
        }
        
        
        if (_spanwManager == null)
        {
            Debug.Log("the spawnmanager is null ");

        }
        if (_uiManager == null)
        {
            Debug.LogError("the uimanager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // palyer1
        calculateMovement();
        
        //if player 
        
        
        spawnlaser();


        AnimatePlayer();



    }


    void calculateMovement()
    {

        float horizontalinput = 0f;
        float verticalinput = 0f;

        if (_isPlayer1)
        {
            if (Input.GetKey(KeyCode.A))
                horizontalinput = -1f;
            else if (Input.GetKey(KeyCode.D))
                horizontalinput = 1f;

            if (Input.GetKey(KeyCode.W))
                verticalinput = 1f;
            else if (Input.GetKey(KeyCode.S))
                verticalinput = -1f;
        }
        else if (_isPlayer2)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                horizontalinput = -1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                horizontalinput = 1f;

            if (Input.GetKey(KeyCode.UpArrow))
                verticalinput = 1f;
            else if (Input.GetKey(KeyCode.DownArrow))
                verticalinput = -1f;
        }

        /*if (_joystick != null)
        {
            // if joystick is being moved, override keyboard input
            if (Mathf.Abs(_joystick.Horizontal) > 0.1f || Mathf.Abs(_joystick.Vertical) > 0.1f)
            {
                horizontalinput = _joystick.Horizontal;
                verticalinput = _joystick.Vertical;
            }
        }*/
        _currentHorizontalInput = horizontalinput;
        
        Vector3 direction = new Vector3(horizontalinput, verticalinput, 0);

        //if speedboost false
        transform.Translate(direction * _speed * Time.deltaTime);
        //else speed boost multiplier
        if (_isSpeedboostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime * _speedmultiplier);
        }
        
        
        
        
        if (transform.position.y >= 5.95f)
        {
            transform.position = new Vector3(transform.position.x, 5.95f, transform.position.z);
        }
        else if (transform.position.y <= -3.97f)
        {
            transform.position = new Vector3(transform.position.x, -3.97f, transform.position.z);
        }

        
        
        //like if bound crossed on left reappear on the other side like a loop
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
        }
        
        
        /**
        //clampimg method to put things into range(a math function)
        //transform.postion=new vector3(transform.postion.x,mathf.clamp(traansform.postion.y,7.65f,-7.65f),transfrom.postion.z);
        **/
    }

    void spawnlaser()
    {
        bool fireInput = false;
        if (_isPlayer1)
        {
            fireInput = Input.GetKeyDown(KeyCode.Space) || _fireButtonPressed;
        }
        else if (_isPlayer2)
        {
            fireInput = Input.GetKeyDown(KeyCode.RightControl);
        }
        
        //spawn object directly under player
        if (fireInput && Time.time>_canfire)
        {
            _canfire = Time.time + _firerate;
            

            //if space key is pressed
            //if tripleshotactive is true
            //fire 3 lasers
            //else fire 3 lasers
            //instantiate 3 lasers(tripleshot prefab)
            if (_isTripleshotActive == true)
            {
                //instantiate triple shot prefab
                Instantiate(_TripleshotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_LaserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            //play the laser audio clip
            _audioSource.Play();

        }
        _fireButtonPressed = false;
    }

    public void Damage()
    {
        //Ignore damage if we were just hit recently
        if (Time.time - _lastDamageTime < _damageCooldown)
        {
            return;
        }

        _lastDamageTime = Time.time;

        //if shields is active 
        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualiser.SetActive(false);
            return;
        }
        else
        {
            _lives--;

            if (_lives == 2)
            {
                int _activeEngineIndex = Random.Range(0, _playerHurt.Length);
                _playerHurt[_activeEngineIndex].SetActive(true);
            }
            else if (_lives == 1)
            {
                for (int i = 0; i < _playerHurt.Length; i++)
                {
                    if (!_playerHurt[i].activeSelf)
                    {
                        _playerHurt[i].SetActive(true);
                        break;
                    }
                }
            }

            _uiManager.UpdateLives(_lives);

            if (_lives < 1)
            {
                _uiManager.CheckForBestScore();
                if (_gameManager != null)
                {
                    _gameManager.OnPlayerDeath();
                }
                Destroy(this.gameObject);
            }
        }
    }

    public void TripleshotActive()
    {
        //triple shot active => true
        //start coroutine for triple shot
        _isTripleshotActive = true;
        StartCoroutine(TripleshotpowerdownRoutine());

    }
    //ienumerator
    //tripleshotpowerdownroutine
    //wait for 5 sec
    //set tripleshot to false
    IEnumerator TripleshotpowerdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleshotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedboostActive = true;
        StartCoroutine(SpeedBoostpowerdownRoutine());
    }
    IEnumerator SpeedBoostpowerdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedboostActive = false;
    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        //enable visualiser
        _shieldVisualiser.SetActive(true);
        
    }
    
    //method to add 10 to score
    //communicate with ui to update score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore();
    }

    public void OnFireButtonPressed()
    {
        _fireButtonPressed = true;
    }
    private void AnimatePlayer()
    {
        // LEFT
        if (_currentHorizontalInput < 0)
        {
            _anim.SetBool("Turn_Left", true);
            _anim.SetBool("Turn_Right", false);
        }
        // RIGHT
        else if (_currentHorizontalInput > 0)
        {
            _anim.SetBool("Turn_Right", true);
            _anim.SetBool("Turn_Left", false);
        }
        // NO HORIZONTAL INPUT → idle
        else
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }
    }
}

    

