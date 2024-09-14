using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    void Start()
    {
        instance = this;
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("level1");
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
