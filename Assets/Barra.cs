using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barra : MonoBehaviour
{
    public Slider slider;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int Health)
    {
        slider.maxValue = Health;
        slider.value = Health;

    }

    public void SetHealth(int Health)
    {
        slider.value = Health;

    }
}
