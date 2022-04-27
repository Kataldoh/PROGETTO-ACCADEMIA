using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlaceholder : MonoBehaviour
{
    public bool hit;
    public int damageTowardsPlayer;
    public float damageTaken;
    [SerializeField] EnemyData eData;
    [SerializeField] bool destroyWhenHealthBelowZERO;
    public float health;


    private void Start() 
    {
        if(eData != null)
        {
            health = eData.health;
        }
            
    }
    private void Update()
    {
        if (hit)
        {
            health -= damageTaken;
            hit = false;
        }

        if (destroyWhenHealthBelowZERO && health <= 0)
            Destroy(gameObject);
    }
}
