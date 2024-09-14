using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerOverObj : MonoBehaviour
{
    public GameControler gControler;

    private TimeLineControler timeLControler;

    public bool trueEnd = false;

    public Cronometro crono;
    public ContadorTempo contaT;

    void Start()
    {
        timeLControler = FindObjectOfType<TimeLineControler>();

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (gControler.totalScoreF >= 20 && gControler.totalScoreC >= 60 && gControler.coletaveis >= 5)
            {
                crono.contando = false;
                contaT.contandoT = false;
                trueEnd = true;
                TimeLineControler.instance.PlayerTimeLine();

                StartCoroutine(waitBeforeShow());
            }
        }
    }

    IEnumerator waitBeforeShow()
    {
        yield return new WaitForSeconds(11);
        GameControler.instance.EndGame();
    }
}
