using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyrData", menuName = "Platform2D/Enemy", order = 1)]

public class EnemyData : ScriptableObject
{
    public float force;
    public float jumpForce;
    public int speedRot;
    public float raycastForward;

}

