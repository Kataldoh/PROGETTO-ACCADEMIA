using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraBoss : MonoBehaviour
{
    public Slider slider;


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealthBoss(int HealthBoss)
    {
        slider.maxValue = HealthBoss;
        slider.value = HealthBoss;

    }

    public void SetHealthBoss(int HealthBoss)
    {
        slider.value = HealthBoss;

    }
}
