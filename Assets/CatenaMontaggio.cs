using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatenaMontaggio : MonoBehaviour
{
    public GameObject fineCatena;
    public Transform spawnPoint;

    public Transform manichino;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    

    void OnTriggerEnter(Collider col)
    {
        
        if (col.transform.gameObject==fineCatena) //quando il manichino tocca la fine, viene respawnato all'inizio
        {
            manichino.position = spawnPoint.position;
        }
    }
    
    
}
