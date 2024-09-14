using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameControler gameContr;

    // Start is called before the first frame update
    void Start()
    {
        gameContr = FindObjectOfType<GameControler>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            gameContr.currentCheckpoint = gameObject;
            Debug.Log("Activated CheckPoint  " + transform.position);
        }
    }
}
