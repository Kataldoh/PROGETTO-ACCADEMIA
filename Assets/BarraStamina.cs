using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraStamina : MonoBehaviour
{
    public Slider slider;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxStamina(float Stamina)
    {
        slider.maxValue = Stamina;
        slider.value = Stamina;

    }

    public void SetStamina(float Stamina)
    {
        slider.value = Stamina;

    }
}
