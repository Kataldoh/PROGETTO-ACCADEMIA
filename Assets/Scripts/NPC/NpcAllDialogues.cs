using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAllDialogues : MonoBehaviour
{
    public string[,] lineeDialogo;

    //lineeDialogo[0,1] = 0-> lingua, 1->dialogo 1

    public string TypeDialogue(int npcID)
    {
        switch (npcID)
        {
            case 1:
                return "NPC tipo 1 - Hey! Dove stai andando? >:(";
            default:
                return "No Dialogue";
        }
    }
}
