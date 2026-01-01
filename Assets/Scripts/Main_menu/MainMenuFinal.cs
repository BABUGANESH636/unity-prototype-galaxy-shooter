using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFinal : MonoBehaviour
{
    public void SinglePlayer()
    {
        SceneManager.LoadScene(1);
    }
    public void CoOpmode()
    {
        SceneManager.LoadScene(2);
    }
}
