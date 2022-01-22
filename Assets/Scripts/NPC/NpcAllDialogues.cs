using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAllDialogues : MonoBehaviour
{
    public void TypeDialogue(int npcID)
    {
        switch (npcID)
        {
            case 1:
                print("Sono un NPC di tipo 1");
                break;
        }
    }
}
