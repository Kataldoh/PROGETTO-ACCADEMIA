using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScript : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 pointerPosition;
    public float slope;
    void Start()
    {
        Cursor.visible = true;   
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Input.mousePosition;
        //mousePos.z = 1;
        pointerPosition = Camera.main.ScreenToWorldPoint(mousePos* slope);
        transform.position = pointerPosition;
    }
}
