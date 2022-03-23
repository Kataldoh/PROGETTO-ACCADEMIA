using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TileStruct {
    public GameObject tile;
    public int dimx = 20;
    public int dimy = 20;
    public bool start;
    public bool end;
}

public class MapEditor : MonoBehaviour
{
    // Start is called before the first frame update


    public List<TileStruct> tile = new List<TileStruct>();

    // int[livello,x,y]
    int[,,] map = 
     {
        {
            { 0,0,0,0},
            { 0,1,1,0},
            { 0,1,1,0},
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
                    Vector3 pos= new Vector3(x * tile[_tile].dimx, -y * tile[_tile].dimy, 0);
                    GameObject go = Instantiate(tile[_tile].tile, pos, transform.rotation);
                    go.transform.parent = transform;
                }
            }
        }

    }

}
