using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    private Animator anim;
    bool canColider = true;
    public ContadorTempo tempo;

    private AudioEffect audioF;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audioF = FindObjectOfType<AudioEffect>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (canColider)
        {
            if (collision.gameObject.tag == "Player")
            {
                audioF.PlaySFX(audioF.audioOxygen);
                canColider = false;
                tempo.minutosG += 2;
                anim.SetBool("add", true);
            }
        }
    }
}
