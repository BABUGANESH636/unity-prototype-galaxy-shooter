using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed=4f;
    private Player _player;

    //handle to animator component
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate=3.0f;
    private float _canFire=-1;

    
    void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogWarning("Enemy could not find any Player in this scene.");
        }

        _audioSource = GetComponent<AudioSource>();
        
        //assign the animator component and null check
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("no component _anim");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser=Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] _lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for(int i = 0; i < _lasers.Length; i++)
            {
                _lasers[i].AssignEnemyLaser();
            }
        }
        


    }

    private void CalculateMovement()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(Random.Range(-8f, 8f), 7, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Player")
        {
            Player player =other.transform.GetComponent<Player>();

            //null checking(if player component exists ot not
            if (player != null)
            {
                player.Damage();
            }
            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject,2.5f);
            
        }
        if (other.tag=="Laser")
        {
            Laser laser = other.GetComponent<Laser>();

            if (laser != null && laser.IsEnemyLaser)
            {
                return;
            }


            Destroy(other.gameObject);
            //add 10 to _score
            if (_player != null)
            {
                _player.AddScore(10);
            }
            //trigger anim
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);

        }
    }

    

}
