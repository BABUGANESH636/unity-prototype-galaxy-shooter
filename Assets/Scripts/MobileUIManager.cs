using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _fireButton;
     

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _fireButton.SetActive(true);
            Debug.Log("this is Android");
        }
        else
        {
            _fireButton.SetActive(false);
            Debug.Log("this is Pc");
        }
    }

}

