using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesEvents : MonoBehaviour
{

    public void _IDLE()
    {

    }

    public void _PLAY()
    {
        GameController.instance.pannelli[0].SetActive(false);
    }

    public void _DEAD()
    {
        GameController.instance.pannelli[0].SetActive(true);

    }

    public void _PAUSE()
    {

    }
}
