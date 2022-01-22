using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc1 : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        this.GetComponent<NpcAllDialogues>().TypeDialogue(1); //getComponent<script> per accedere ai metodi di quest'ultimo script
    }
}
