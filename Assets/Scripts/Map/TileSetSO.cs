using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tileset", menuName = "Platform2D/Tileset", order = 5)]
public class TileSetSO : ScriptableObject
{
    public List<TileStruct> tileset = new List<TileStruct>();
}
