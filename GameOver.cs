using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public int totalScoreFlor;
    public Text scoreTextF;

    public int totalScoreCristal;
    public Text scoreTextC;

    public static GameOver instance;

    void Start()
    {
        instance = this;
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
