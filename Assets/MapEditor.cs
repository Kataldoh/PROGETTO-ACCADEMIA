using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    // Start is called before the first frame update


    public List<GameObject> tile = new List<GameObject>();
    [SerializeField] int dimTile;

    // int[livello,x,y]
    int[,,] map = 
     {
        {
            { 0,3,3,3},
            { 1,0,1,1},
            { 0,0,0,0},
            { 0,0,0,0}
        },
        {
            { 0,3,3,3},
            { 1,1,1,1},
            { 0,0,0,0},
            { 0,0,0,0}
        }
     };



    void Start()
    {
 
        for (int x = 0; x < map.GetLength(2); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int _tile = map[0, y, x];
                if (_tile > -1)
                {
                    GameObject go = Instantiate(tile[_tile], transform.position, transform.rotation);
                    go.transform.position = new Vector3(x * dimTile, -y * dimTile, 0);
                    go.transform.parent = transform;
                }
            }
        }

    }

}
