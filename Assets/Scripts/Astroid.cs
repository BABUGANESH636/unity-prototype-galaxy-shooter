using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Astroid : MonoBehaviour
{
    // Start is called before the first frame update
    private float _rotate_speed = 20.0f;
    [SerializeField]
    private GameObject explosionPrefab;
    private Spawn_Manager _spawnManager;
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate the astroid along z axis
        transform.Rotate(Vector3.forward * _rotate_speed * Time.deltaTime);
    }

    //check for laser collision
    //instantiate explosion at the position of(us)
    //destroy the explosion after three seconds
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject,0.25f);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
        }
        
    }
}
