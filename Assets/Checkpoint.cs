using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            SaveSystem.instance.saving = true;
        }
        //Sistrema DI morte Assente
        //Da aggiungere pe Caricamento
    }
}
