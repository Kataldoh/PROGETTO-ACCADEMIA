using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlaceholder : MonoBehaviour
{
    public bool hit;
    public float damage;
    [SerializeField] BossCode b;

    private void Update()
    {
        if (hit)
        {
            b.health -= damage;
            hit = false;
        }
    }
}
