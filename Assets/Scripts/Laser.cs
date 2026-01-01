using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    private float _speed = 8f;

    public bool IsEnemyLaser
    {
        get { return _isEnemyLaser; }
    }


    private bool _isEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }


    
   

    void MoveUp()
    {
        //traslate laser north
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //destroy laser once it goes off screen
        if (transform.position.y > 8f)
        {
            //check if this object has a parent
            //destroy the parent too
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);


        }
    }

    void MoveDown()
    {
        //traslate laser north
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //destroy laser once it goes off screen
        if (transform.position.y < -8f)
        {
            //check if this object has a parent
            //destroy the parent too
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);


        }

    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("No player component");
                return;
            }

            player.Damage();
            Debug.Log("Enemy laser hit: " + other.name);


            // destroy this laser after hit
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

}
