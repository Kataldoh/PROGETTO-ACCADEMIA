using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proiettile : MonoBehaviour
{
    public float speed;

    private Transform player;
    private Vector3 target;
    

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;//altrimenti i proiettili andrebbero in un'unica direzione

        target = new Vector3(player.position.x, player.position.y, player.position.z);// target = posizione del player
    }

    
    void Update()
    {
        
        // il p. si muove verso il player
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //se le coordinate del proiettile sono uguali a quelle del player, allora il proiettile si degenera
            if (transform.position.x == target.x && transform.position.y == target.y && transform.position.z == target.z)
            {
                Distruggiproiettile();
            }
        
    }

    private void OnTriggerEnter(Collider other)// se collide con il player allora si degenera
    {
        if (other.CompareTag("Player"))
        {
            Distruggiproiettile();
        }
    }

    void Distruggiproiettile()//distruggo il proiettile
    {
        Destroy(gameObject);
    }


}
