using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Platform2D/Save", order = 5)]
public class SaveData : ScriptableObject
{
    //Contenitore info player 
    public float player_health;
    public float[] player_position;
}
