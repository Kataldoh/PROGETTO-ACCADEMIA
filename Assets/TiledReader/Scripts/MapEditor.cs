using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FabTiledJsonReader; // mia lib :)

public class MapEditor : MonoBehaviour
{
    //****** Struttura del singolo Tile
    
    public TileSetSO[] TileDataStruct;
    // *********************************

    //File da caricare in /Assets/Resources
    public TextAsset TiledJsonData; 

    //Carico la "libreria"
    //Decommentare [serializefield], per mostrare nell'inspector il contenuto del JSON
    
    //[SerializeField]
    TiledData tdata = new TiledData();

    [SerializeField] GameObject Placeholder;

    //[SerializeField] int LayerID;
    int[] MapData;

    [SerializeField] bool GenerateScriptable;

    void Start()
    {

        LoadData();

        LoadTileStruct();


        if (GenerateScriptable)
        {
            for (int n = 0; n < tdata.tilesets.Length; n++)
            {
                try
                {
                    print("Genero: " + tdata.tilesets[n].source);
                    GenerateTileStruct(n);
                    print(tdata.tilesets[n].source + " ...Generato");
                }
                catch
                {
                    print("Errore nella generazione della struttura tiles");
                }
            }
        }

        for (int n = 0; n < tdata.layers.Length; n++)
        {

            //Controllo che siano DATA
            if (tdata.layers[n].data != null)
                RenderMap(n);
            //altrimenti sono Objects
            else
            {
                RenderObjects(n);
            }
        }

    }


    void LoadData() {

        // Carico la struttura JSON.
        // P.s. Il file DEVE risiedere in /Assets/Resources (creare Resources se non esiste)
        tdata = JsonUtility.FromJson<TiledData>(TiledJsonData.text);
        print("Mappa generata con Tiled versione: " + tdata.tiledversion);
        print("Tilesets:" + tdata.tilesets.Length);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    void RenderObjects(int id)
    {
        GameObject _layer = new GameObject("Layer: " + tdata.layers[id].name);
        for (int o = 0; o < tdata.layers[id].objects.Length; o++)
        {
            GameObject _obj = new GameObject(tdata.layers[id].objects[o].name);
            _obj.transform.parent = _layer.transform;
            _obj.transform.position = new Vector3(tdata.layers[id].objects[o].x / 100, -tdata.layers[id].objects[o].y / 100, 0);
        }
        _layer.transform.parent = transform;
    }



    /// <summary>
    /// Renderizza la mappa a video
    /// </summary>
    /// <param name="ID"> Id Layer</param>
    void RenderMap(int ID) {

        MapData = tdata.layers[ID].data;

        //Creo un GameObject vuoto e gli passo il nome del layer
        GameObject _layer = new GameObject("Layer: " + tdata.layers[ID].name);

        for (int x = 0; x < tdata.layers[ID].width; x++)
        {
            for (int y = 0; y < tdata.layers[ID].height; y++)
            {
                var t = MapData[x + y * tdata.layers[ID].width]-1;
                if (t > -1)
                {
                    print("*** ID: " + ID);
                    var NextTileSet = t - (tdata.tilesets[ID].firstgid - 1);
                    print("Tile type: " + NextTileSet.ToString());
                    var dataScriptable = TileDataStruct[ID].tileset[NextTileSet];

                    //centra la mappa
                    float posx = x-tdata.layers[ID].width * 0.5f;
                    float posy = -y+tdata.layers[ID].height * 0.5f;


                    Vector2 tilePosition = new Vector3(posx, posy);
                    GameObject go = Instantiate(dataScriptable.prefab, tilePosition, Quaternion.identity);

                    //Buggato
                    //go.layer = 1 << tiles.tileset[t].layer;

                    //metto il prefab creato dentro al gameobject precedentemente creato, con il nome del layer attuale.
                    if (t == 9)
                    {
                        Destroy(go.GetComponent<BoxCollider>());
                    }
                    go.transform.parent = _layer.transform;
                        
                }
            }
        }

        //posto gameobject precedentemente creato, dentro al padre.
        _layer.transform.parent = transform;

        //attivo o disattivo il layer, come specificato da tiled
        _layer.gameObject.SetActive(tdata.layers[ID].visible);
    }

    void GenerateTileStruct(int t)
    {

        TileSetSO o = ScriptableObject.CreateInstance<TileSetSO>();
        o.tileset.Add(new TileStruct
        {
            prefab = Placeholder
        });

        AssetDatabase.CreateAsset(o, Spath(t));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();


    }


    void LoadTileStruct()
    {

        TileDataStruct = new TileSetSO[tdata.tilesets.Length];
        for (int n = 0; n < TileDataStruct.Length; n++)
        {
            try
            {
                TileDataStruct[n] = AssetDatabase.LoadAssetAtPath<TileSetSO>(Spath(n));
            }
            catch
            {
                print("NON TROVO IL FILE!");
            }
        }
    }

    string Spath(int t)
    {
        return "Assets/Scripts/Map/ScriptableObject/" + tdata.tilesets[t].source + ".asset";
    }

}
