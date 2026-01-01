using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    private float _speed = 3.0f;
    //ID for powerups
    //0 - Tripleshot
    //1 - Speed
    //2 - Shields
    [SerializeField]
    private int _powerupID;
    
    [SerializeField]
    private AudioClip _audioClip;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at the speed of 3(adjust in inspectpr)
        //when we leave the screen destroy this object
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.76f)
        {
            Destroy(this.gameObject);
        }
        
    }

    //ontriggercollision 
    //only be collectable by the player //use tags
    //on collected ,destroy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //communicate with player script
            //handle the component we want
            //player component
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleshotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("speed powerup");
                        break;
                    case 2:
                        player.ShieldActive();
                        Debug.Log("shield powerup");
                        break;
                    default:
                        Debug.Log("default vaue");
                        break;
                }
                
            }
            Destroy(this.gameObject);
        }
        
    }
}
