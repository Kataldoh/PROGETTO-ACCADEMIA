using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAllDialogues : MonoBehaviour
{
    public string[] lineeDialogo;

    public string TypeDialogue(int npcID)
    {
        switch (npcID)
        {
            case 1:
                return "sono un npc di tipo 1";
            default:
                return "No Dialogue";
        }
    }
}
