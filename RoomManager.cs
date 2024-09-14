using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCam;

    void OnTriggerEnter2D(Collider2D collisio)
    {
        if (collisio.CompareTag("Player") && !collisio.isTrigger)
        {
            virtualCam.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collisio)
    {
        if (collisio.CompareTag("Player") && !collisio.isTrigger)
        {
            virtualCam.SetActive(false);
        }
    }
}
