using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cronometro : MonoBehaviour
{
    //public Text textSegundos;
    //public Text textMinutos;
    public int limiteSegundos;
    public float segundos;
    public int minutos;

    public Text segundosFinal;
    public Text minutosFinal;

    public bool contando = true;


    // Update is called once per frame
    void Update()
    {
        segundosFinal.text = segundos.ToString("00");
        minutosFinal.text = minutos.ToString("00");
        if (contando)
        {
            segundos += Time.deltaTime;
            if (segundos >= limiteSegundos)
            {
                minutos++;
                segundos = 0 + 1;
            }
        }

    }
}
