using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapEditor : MonoBehaviour
{
    // Start is called before the first frame update


    public List<TileStruct> tile = new List<TileStruct>();
    [SerializeField] int LevelID;

    // int[livello,x,y]
    int[,,] map = 
     {

        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 36, 1, 1, 1, 1, 39, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 71, 0, 0, 0, 0, 0, 0 },
            { 1, 1, 39, 0, 0, 36, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 36, 1, 1, 1, 1, 39, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 39, 0, 0, 36, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            { 0, 36, 1, 1, 1, 1, 39, 0, 0, 0, 82, 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0 },
            { 1, 1, 39, 0, 0, 36, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
          }

     };



    void Start()
    {
 
        for (int x = 0; x < map.GetLength(2); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int _tile = map[LevelID, y, x];
                if (_tile<2)
                {
                    Vector3 pos= new Vector3(x * tile[_tile].dimx, -y * tile[_tile].dimy, 0);
                    GameObject go = Instantiate(tile[_tile].tile, pos, transform.rotation);
                    go.transform.parent = transform;
                }
            }
        }

    }

}
