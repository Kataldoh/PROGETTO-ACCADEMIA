using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlaceholder : MonoBehaviour
{
    public bool hit;
    public float damage;
    [SerializeField] EnemyData eData;
    public float health;

    private void Start() 
    {
        health = eData.health;
    }
    private void Update()
    {
        if (hit)
        {
            health -= damage;
            hit = false;
        }
    }
}
