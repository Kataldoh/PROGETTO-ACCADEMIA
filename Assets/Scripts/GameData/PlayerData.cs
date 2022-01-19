using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Platform2D/Player", order = 1)]

public class PlayerData : ScriptableObject
{
    public float force;
    public float jumpForce;
    public int speedRot;
    public float raycastForward;

}
