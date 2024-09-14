using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContadorTempo : MonoBehaviour
{
    public Text segundosText;
    public Text minutosText;
    public int limiteSegundosG;
    public float segundosG;
    public int minutosG;

    public bool contandoT = true;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        segundosText.text = segundosG.ToString("00");
        minutosText.text = minutosG.ToString("00");
        if (contandoT)
        {
            if (minutosG >= 0 && segundosG > 0)
            {
                segundosG -= Time.deltaTime;
                if (segundosG <= limiteSegundosG)
                {
                    minutosG--;
                    segundosG = 0 + 60;
                }
            }
            else
            {
                minutosG = 0;
                segundosG = 0;
                GameControler.instance.EndGame();
            }
        }
    }
}
