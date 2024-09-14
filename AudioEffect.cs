using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour
{
    public static AudioEffect instance;

    private AudioSource audioSource;

    public AudioClip audioJump;
    public AudioClip audioCristal;
    public AudioClip audioFlor;
    public AudioClip audioArtefato;
    public AudioClip audioWater;
    public AudioClip audioOxygen;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(AudioClip sfx)
    {
        audioSource.PlayOneShot(sfx);
    }
}
