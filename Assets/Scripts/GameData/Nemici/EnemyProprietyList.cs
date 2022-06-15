using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Proprieties", menuName = "Platform2D/Enemy Proprieties", order = 6)]
public class EnemyProprietyList : ScriptableObject
{
    /*
     ORDINE DEGLI OGGETTI NELL'ARRAY
    0-Fuoco
    1-Crawler
    2-Walker
     
     
     */


    public List<EnemyData> enemyDataList = new List<EnemyData>();
}
