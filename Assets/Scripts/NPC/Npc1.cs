using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //per poter modificare i testi

public class Npc1 : MonoBehaviour
{
    //variabili dialogo
    public GameObject pannelloDialogo; //il pannello dove il dialogo appare
    public TextMeshProUGUI dialogueBox;
    NpcAllDialogues dialoghi = new NpcAllDialogues(); //dichiaro una variabile di tipo NpcDialogues cos� da poter invocare il medodo in quello script


    //variabili raycast
    bool hittedForward, hittedBehind;
    RaycastHit hitForward, hitBehind;

    void Start()
    {
        //dialoghi.TypeDialogue(1); //definisco che tipo di npc questo � e lo mando allo script con tutte le linee di dialogo
        pannelloDialogo.SetActive(false);
    }

    void Update()
    {
        
        //quando il player si avvicina abbastanza parte la possibilità di far parlare l'npc
        hittedForward = Physics.Raycast(this.transform.position, this.transform.forward, out hitForward, 2);
        hittedBehind = Physics.Raycast(this.transform.position, -this.transform.forward, out hitBehind, 2);

        if (hittedForward || hittedBehind) //se rileva qualcuno davanti o dietro
        {
            if (hitForward.collider.tag == "Player") //se ciò che si collide è il player
            {
                print("il player mi ha toccato");
                
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E)) //esce pannello di dialogo
        {
            pannelloDialogo.SetActive(true);
            dialogueBox.text = dialoghi.TypeDialogue(1); //modifico il testo a schermo con quello del dialogo appropriato 
        }

    }
}
