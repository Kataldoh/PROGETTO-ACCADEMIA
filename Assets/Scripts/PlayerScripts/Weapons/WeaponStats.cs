using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "Platform2D/Weapon", order = 3)]
public class WeaponStats : ScriptableObject
{
    public bool isRaycast;
    public float damage;
    public float shootingInterval_inSeconds;
    public GameObject projectileOrTrailModel_Prefab;
    public float weaponRange;
    public Vector2 knockbackToPlayer;
    public Vector2 knockbackToEnemy;
}
