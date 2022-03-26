// $Ver 0.001 by Fabrizio Radica 2022
//************** Struttura base JSon Tiled *****
// La struttura di queste classi è definita dentro al file Json di Tiled
// *** IMPORTANTE ***
// da Tiled esportare in .txt!!!!
namespace FabTiledJsonReader
{

    [System.Serializable]
    public class TiledData
    {
        public layers[] layers;
        public string type;
        public string tiledversion;
        public int tileheight;
        public int tilewidth;
    }

    [System.Serializable]
    public class layers
    {
        public int[] data; // qui risiede la mappa dei tiles
        public int height; // dimensione della mappa in Y...
        public int id;
        public string name; //Nome della mappa
        public string type;
        public bool visible;
        public int width;  // ... e in X
        public int x; // posizione X
        public int y; // posizione Y
    }
}
// *********************************************
