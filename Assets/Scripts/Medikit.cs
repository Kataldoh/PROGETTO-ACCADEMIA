using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public int cura;
    private void OntriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            //richiamo il metodo TakeDamage
            GameController.instance.TakeDamage(-cura);
            Destroy(collision.gameObject);
        }
    }
}
