using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //per poter modificare i testi

public class Npc1 : MonoBehaviour
{
    public GameObject pannelloDialogo;
    public TextMeshProUGUI dialogueBox;

    NpcAllDialogues dialoghi = new NpcAllDialogues(); //dichiaro una variabile di tipo NpcDialogues cos� da poter invocare il medodo in quello script

    void Start()
    {
        //dialoghi.TypeDialogue(1); //definisco che tipo di npc questo � e lo mando allo script con tutte le linee di dialogo
        pannelloDialogo.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            pannelloDialogo.SetActive(true);
            dialogueBox.text = dialoghi.TypeDialogue(1); //modifico il testo a schermo con quello del dialogo appropriato   dialoghi.lineeDialogo[0]
        }
        
    }
}
