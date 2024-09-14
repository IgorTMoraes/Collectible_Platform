using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineControler : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnim;

    public static TimeLineControler instance;

    private PlayableDirector directoTime;
    private RuntimeAnimatorController controlador;
    private bool consertado = false;

    // Start is called before the first frame update
    void Awake()
    {
        directoTime = GetComponent<PlayableDirector>();
        instance = this;

    }

    private void OnEnable()
    {
        controlador = playerAnim.runtimeAnimatorController;
        playerAnim.runtimeAnimatorController = null;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (directoTime.state != PlayState.Playing  && !consertado)
        {
            playerAnim.runtimeAnimatorController = controlador;
            consertado = true;
         // GameControler.instance.EndGame();
        }
    }

    public void PlayerTimeLine()
    {
        directoTime.Play();     
    }
}
