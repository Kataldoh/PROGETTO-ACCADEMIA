using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAllDialogues : MonoBehaviour
{
    public string[] lineeDialogo;

    public void TypeDialogue(int npcID)
    {
        switch (npcID)
        {
            case 1:
                lineeDialogo[0] = "sono un npc di tipo 1";
                break;
        }
    }
}
