using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlaceholder : MonoBehaviour
{
    public int ID_entity;
    public bool hit;
    public int damageTowardsPlayer;
    public float damageTaken;
    [SerializeField] EnemyProprietyList eData;
    [SerializeField] bool destroyWhenHealthBelowZERO;
    public float health;


    private void Start() 
    {
        if(eData != null)
        {
            health = eData.enemyDataList[ID_entity].health;
            damageTowardsPlayer = eData.enemyDataList[ID_entity].damage;
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
