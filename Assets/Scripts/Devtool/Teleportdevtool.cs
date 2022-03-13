using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportdevtool : MonoBehaviour
{
    public GameObject player;
    [SerializeField] Transform[] checkpoint;
    void Update()
    {
        //teletrasporto all'inizio del livello
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            player.transform.position = checkpoint[0].position;
        }

        //teletrasporto alla seconda sezione
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            player.transform.position = checkpoint[1].position;
        }

        //teletrasprto alla terza sezione
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            player.transform.position = checkpoint[2].position;
        }

        //teletrasporto alla quarta sezione
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            player.transform.position = checkpoint[3].position;
        }
        //powerup n1
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            player.transform.position = checkpoint[4].position;
        }

        //
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            player.transform.position = checkpoint[5].position;
        }

        //
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            player.transform.position = checkpoint[6].position;
        }

        //
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            player.transform.position = checkpoint[7].position;
        }

        //
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            player.transform.position = checkpoint[8].position;
        }

        //
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            player.transform.position = checkpoint[9].position;
        }

    }
}
