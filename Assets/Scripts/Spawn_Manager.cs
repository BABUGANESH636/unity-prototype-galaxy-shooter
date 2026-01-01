using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _enemyprefab;
    [SerializeField]
    private GameObject _enemycontainer;
    private bool _stopspwaning=false;
    [SerializeField]
    private GameObject[] _powerups;



    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //spawn an gameobject every 5 sec
    //create a coroutine of type IEnumerator --Yield events
    //while loop(infinite)

    IEnumerator SpawnEnemyRoutine()  //allows us to use "yield"  
    {
        yield return new WaitForSeconds(3.0f);
        //while loop(infinite)
        //Instantite enemyprefab
        //yield wait 5 sec
        while (_stopspwaning== false)
        {
            Vector3 postospawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            // { this way we dont have any acces to the objects that just spawned} Instantiate(_enemyprefab,postospawn,Quaternion.identity);
            //we must keep the structure clean thus we r gonna store the instantiated obj into a parent / comtainer in our hirearchy
            GameObject _newenemy= Instantiate(_enemyprefab, postospawn, Quaternion.identity);
            _newenemy.transform.parent = _enemycontainer.transform;
            
            yield return new WaitForSeconds(5.0f);

        }
       
    
    }
    public void StopSpawning()
    {
        _stopspwaning = true;
        Debug.Log("Player died. Stopping enemy spawns!");
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        //every 3-7seconds spawn in an powerup
        while (_stopspwaning == false)
        {
            Vector3 postospawnpowerup = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randompowerups = Random.Range(0, 3);
            GameObject _newpowerups = Instantiate(_powerups[randompowerups], postospawnpowerup, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }
}