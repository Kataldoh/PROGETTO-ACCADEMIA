using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling_infinite : MonoBehaviour
{
    public MeshRenderer mr;
    public float speed;

    //Ogni secondo l'offset della textre aumenta di uno
    void Update()
    {
        mr.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}

