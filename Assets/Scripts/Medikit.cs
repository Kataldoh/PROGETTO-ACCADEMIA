using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medikit: MonoBehaviour
{

    public int cura;
    private void OntriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            //richiamo il metodo TakeDamage
            GameController.instance.TakeDamage((int)-GameController.instance.medikitEnergy[0]);
            Destroy(collision.gameObject);
        }
    }
}
