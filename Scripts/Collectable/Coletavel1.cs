using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel1 : MonoBehaviour
{
    private SpriteRenderer sr;
    private CapsuleCollider2D caps;

    public GameObject collected;
    public int Score;

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
            audioF.PlaySFX(audioF.audioFlor);
            sr.enabled = false;
            caps.enabled = false;

            GameControler.instance.totalScoreF += Score;
            GameControler.instance.UpdateScoreText();
            Debug.Log(Score);

            Destroy(gameObject);
        }
    }
}
