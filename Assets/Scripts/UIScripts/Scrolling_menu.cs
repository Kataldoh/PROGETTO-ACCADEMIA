using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling_menu : MonoBehaviour
{
    //public GameObject spawn;
    public GameObject check;
    public GameObject ritorno;

    public float speed;

    public void Start()
    {
        //spawn = GameObject.Find("Spawn");
        check = GameObject.Find("Check");
        ritorno = GameObject.Find("Return");
        speed = 50;
    }

    public void Update()
    {
        transform.Translate(-Time.deltaTime*speed, 0, 0);
        /*if (check.transform.position == ritorno.transform.position)
        {
            transform.position = spawn.transform.position;
        }*/
    }

}
