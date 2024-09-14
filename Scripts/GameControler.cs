using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    public int totalScoreF;
    public Image florI;
    public Text scoreTextF;

    public int totalScoreC;
    public Image cristalI;
    public Text scoreTextC;

    public Image special1;
    public Image special2;
    public Image special3;
    public Image special4;
    public Image special5;

    public Image validador1;
    public Image validador2;
    public Image validador3;
    public Image validador4;
    public Image validador5;
    public Image validador6;
    public Image validador7;

    public Image endText;
    public bool valido = true;

    public int coletaveis;

    public Text scoreCristalF;
    public Text scoreFlorF;
    public Text artifctF;
    public Text rank;

    public static GameControler instance;
    public GameOver gameOver;
    public Menu menu;
    public Cronometro crono;
    public ContadorTempo contaT;
    public GamerOverObj objEnd;

    public GameObject currentCheckpoint;
    private PlayerTeste player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerTeste>();
        instance = this;
    }

    private void FixedUpdate()
    {
        EndText();
        Menu();
        EndRank();
    }

    public void UpdateScoreText()
    {
        if(totalScoreF >= 1)
        {
            florI.gameObject.SetActive(true);
            scoreTextF.gameObject.SetActive(true);
            scoreTextF.text = totalScoreF.ToString("00");
            if (totalScoreF == 20)
            {
                validador2.gameObject.SetActive(true);
            }
        }

        if (totalScoreC >= 1)
        {
            cristalI.gameObject.SetActive(true);
            scoreTextC.gameObject.SetActive(true);
            scoreTextC.text = totalScoreC.ToString("00");
            if (totalScoreC == 60)
            {
                validador1.gameObject.SetActive(true);
            }
        }

        scoreFlorF.text = totalScoreF.ToString("00");
        scoreCristalF.text = totalScoreC.ToString("00");
        artifctF.text = coletaveis.ToString("0");
    }

    public void Collected(int number)
    {
        if (number == 1)
        {
            special1.gameObject.SetActive(true);
            validador3.gameObject.SetActive(true);
        }
        if (number == 2)
        {
            special2.gameObject.SetActive(true);
            validador4.gameObject.SetActive(true);
        }
        if (number == 3)
        {
            special3.gameObject.SetActive(true);
            validador5.gameObject.SetActive(true);
        }
        if (number == 4)
        {
            special4.gameObject.SetActive(true);
            validador6.gameObject.SetActive(true);
        }
        if (number == 5)
        {
            special5.gameObject.SetActive(true);
            validador7.gameObject.SetActive(true);
        }
    }

    public void EndText()
    {
        if (totalScoreF == 20 && totalScoreC == 60 && coletaveis == 5 && valido)
        {
            endText.gameObject.SetActive(true);
            crono.contando = false;
            contaT.contandoT = false;
            player.speed = 0;
            player.jumpForce = 0;
            StartCoroutine(waitBeforeShow());

        }
    }

    IEnumerator waitBeforeShow()
    {
        yield return new WaitForSeconds(6);
        endText.gameObject.SetActive(false);
        valido = false;
        player.speed = 5;
        player.jumpForce = 13;
        crono.contando = true;
        contaT.contandoT = true;
    }

    public void EndRank()
    {
        if (totalScoreF < 8 && totalScoreC < 30 && coletaveis < 2)
        {
            rank.text = "F";
        }

        if (totalScoreF >= 8 && totalScoreC >= 30 && coletaveis >= 2)
        {
            rank.text = "E";
        }

        if (totalScoreF >= 10 && totalScoreC >= 35 && coletaveis >= 2)
        {
            rank.text = "D";
        }

        if (totalScoreF >= 15 && totalScoreC >= 45 && coletaveis >= 3)
        {
            rank.text = "C";
        }

        if (totalScoreF >= 18 && totalScoreC >= 50 && coletaveis >= 4)
        {
            rank.text = "B";
        }

        if (totalScoreF >= 20 && totalScoreC >= 50 && coletaveis >= 5)
        {
            rank.text = "A";
        }

        if (totalScoreF >= 20 && totalScoreC >= 60 && coletaveis >= 5)
        {
            rank.text = "S";
        }
    }

    public void RespawnPlayer()
    {
        Debug.Log("Player Respawn");
        player.transform.position = currentCheckpoint.transform.position;
    }

    public void Menu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void EndGame()
    {
        gameOver.gameObject.SetActive(true);
        gameOver.totalScoreFlor = totalScoreF;
        gameOver.totalScoreCristal = totalScoreC;
        crono.contando = false;
        contaT.contandoT = false;
        Time.timeScale = 0;
    }
}
