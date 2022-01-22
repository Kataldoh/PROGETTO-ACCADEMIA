using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAllDialogues : MonoBehaviour
{
    
    public void TypeDialogue(int npcID, string[] lineeDialogo)
    {
        switch (npcID)
        {
            case 1:
                lineeDialogo[1] = "sono un npc di tipo 1";
                break;
        }
    }
}
