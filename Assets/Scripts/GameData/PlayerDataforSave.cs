using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataforSave : MonoBehaviour
{
    public float[] playerposition;

    public PlayerDataforSave(MainPlayerScript player)
    {
        playerposition = new float[3];
        playerposition[0] = player.transform.position.x;
        playerposition[1] = player.transform.position.y;
        playerposition[2] = player.transform.position.z;
    }
}
