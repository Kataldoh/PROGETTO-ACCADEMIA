using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc1 : MonoBehaviour
{
    NpcAllDialogues dialoghi = new NpcAllDialogues(); //dichiaro una variabile di tipo NpcDialogues
    string[] dialogo; //in questo array � dove andr� il dialogo

    void Start()
    {
        dialoghi.TypeDialogue(1, dialogo); //carico il dialogo nell'array
    }

    void Update()
    {

    }
}
