using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FabTiledJsonReader; // mia lib :)

public class MapEditor : MonoBehaviour
{
    //****** Struttura del singolo Tile
    
    public TileSetSO tiles;
    // *********************************

    //File da caricare in /Assets/Resources
    public TextAsset TiledJsonData; 

    //Carico la "libreria"
    //Decommentare [serializefield], per mostrare nell'inspector il contenuto del JSON
    
    //[SerializeField]
    TiledData tdata = new TiledData();

    [SerializeField] int LayerID;
    int[] MapData;

    void Start()
    {
        // Carico la struttura JSON.
        // P.s. Il file DEVE risiedere in /Assets/Resources (creare Resources se non esiste)
        tdata = JsonUtility.FromJson<TiledData>(TiledJsonData.text);

        MapData = tdata.layers[LayerID].data;
        print("Mappa generata con Tiled versione: " + tdata.tiledversion);
        RenderMap(LayerID);
    }

    /// <summary>
    /// Renderizza la mappa a video
    /// </summary>
    /// <param name="ID"> Id Layer</param>
    void RenderMap(int ID) {
        for (int x = 0; x < tdata.layers[ID].width; x++)
        {
            for (int y = 0; y < tdata.layers[ID].height; y++)
            {
                var t = MapData[x + (y * tdata.layers[ID].height)]-1;
                if (t > -1)
                {
                    //centra la mappa
                    float posx = x-tdata.layers[ID].width/2;
                    float posy = -y+tdata.layers[ID].height/2;

                    Vector2 tilePosition = new Vector3(posx, posy);
                    GameObject go = Instantiate(tiles.tileset[t].prefab, tilePosition, Quaternion.identity);
                    go.layer = 1 << tiles.tileset[t].layer;
                    go.transform.parent = transform;
                }
            }
        }
    }

}
