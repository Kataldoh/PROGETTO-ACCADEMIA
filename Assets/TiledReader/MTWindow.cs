using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextEditor))]
public class MTWindow : EditorWindow
{
    [MenuItem("MTeditor/Editor")]
    public static void CustomEditorWindow()
    {
        GetWindow<MTWindow>("Editor Window");
    }
    bool isWrited;

    void OnGUI()
    {

        var TEScript = MTTIleEditor.instance;
        Event e = Event.current;

        GUILayout.Space(8);
        GUILayout.Label("Versione mappa caricata "+MapEditor.instance.tdata.tiledversion);



        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.white;

        if (GUILayout.Button("Render livello",style)) {
            if (TEScript.MappaLivello)
            {


                var path = "Assets/Prefabs/Livelli/";

                //PrefabUtility.SaveAsPrefabAsset(TEScript.MappaLivello, path, out isWrited);
                PrefabUtility.SaveAsPrefabAssetAndConnect(TEScript.MappaLivello, path, InteractionMode.AutomatedAction);

                if (isWrited)
                {
                    GUILayout.Label("Prefab "+ TEScript.MappaLivello.name+" salvato...");
                }
                else
                {
                    GUILayout.Label("Errore si salvataggio");
                }

                EditorUtility.FocusProjectWindow();
            }

        }
    }
        // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}