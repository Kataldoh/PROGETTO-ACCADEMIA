using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcSObject", menuName = "Platform2D/Npc", order = 2)]

public class NpcSObject : ScriptableObject
{
    public string[,] dialoghi; //dichiaro matriche contenente i dialoghi (primo valore indica la lingua, secondo il contenuto del dialogo)

    public void caricamento()
    {
        dialoghi[0, 0] = "NPC lingua 1, tipo 1";
    }
     
}
