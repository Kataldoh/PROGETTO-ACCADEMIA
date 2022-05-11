using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_menu_scrolling : MonoBehaviour
{
    public GameObject scrolling_menu;
    public GameObject spawn;

    public void Start()
    {
        scrolling_menu = GameObject.Find("ScrollImageMenu");
        spawn = GameObject.Find("Spawn");
    }
    private void OnCollisionEnter(Collision collision)
    {
        scrolling_menu.transform.position = spawn.transform.position;
    }
}