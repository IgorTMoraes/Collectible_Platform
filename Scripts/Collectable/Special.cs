using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D colli;

    public int number;

    private AudioEffect audioF;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        colli = GetComponent<Collider2D>();
        audioF = FindObjectOfType<AudioEffect>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            audioF.PlaySFX(audioF.audioArtefato);
            sr.enabled = false;
            colli.enabled = false;

            GameControler.instance.Collected(number);
            GameControler.instance.coletaveis += 1;

            Destroy(gameObject);
        }
    }
}
