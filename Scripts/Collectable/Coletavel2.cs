using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel2 : MonoBehaviour
{
    private SpriteRenderer sr;
    private CapsuleCollider2D caps;

    public GameObject collected;
    public int Score2;

    private AudioEffect audioF;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        caps = GetComponent<CapsuleCollider2D>();
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
            audioF.PlaySFX(audioF.audioCristal);
            sr.enabled = false;
            caps.enabled = false;
            collected.SetActive(true);

            GameControler.instance.totalScoreC += Score2;
            GameControler.instance.UpdateScoreText();


            Destroy(gameObject, 0.25f);


        }
    }
}
