using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


//In questa classe verranno inserite tutte le variabili che verranno trattentute prima che vengano salvate
public class PlayerDataforSave 
{
    public float[] playerposition;

    //classe per contenere data del player
    public PlayerDataforSave(MainPlayerScript player)
    {
        playerposition = new float[3];
        playerposition[0] = player.transform.position.x;
        playerposition[1] = player.transform.position.y;
        playerposition[2] = player.transform.position.z;
    }
}
