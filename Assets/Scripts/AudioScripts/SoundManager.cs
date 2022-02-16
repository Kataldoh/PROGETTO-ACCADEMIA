using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace UnityCore
{
    namespace Audio
    {
         public class SoundManager : MonoBehaviour
         {


            public static SoundManager instance;


            public bool debug;
            public AudioTrack[] tracks;


            private Hashtable AudioTable; // relazione tra tipi di audio e traccie audio
            private Hashtable JobTable;


            [System.Serializable]
            public class AudioObject
            {
                public AudioType type;
                public AudioClip clip;
            }

            [System.Serializable]
            public class AudioTrack
            {
                public AudioSource source;
                public AudioObject[] audio;
            }

            private class AudioFunction
            {
                public AudioAction action;
                public AudioType type;

                public AudioFunction(AudioAction _action,AudioType _type)
                {
                    action = _action;
                    type = _type;
                }
            }

            private enum AudioAction
            {
                START,
                STOP,
                RESTART,

            }

            #region Metodi Unity

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            private void Awake()
            {
                if (!instance)
                {
                    Configure();
                }
            }


            private void OnDisable()
            {
                Dispose();
            }
            #endregion

            #region Metodi Pubblici

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            public void PlayAudio(AudioType type)
            {
                AddFunction(new AudioFunction(AudioAction.START,type));
            }
            public void StopAudio(AudioType type)
            {
                AddFunction(new AudioFunction(AudioAction.STOP, type));

            }
            public void RestartAudio(AudioType type)
            {
                AddFunction(new AudioFunction(AudioAction.RESTART, type));

            }


            #endregion

            #region Metodi Privati

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            private void Configure()
            {
                instance = this;

                AudioTable = new Hashtable();
                JobTable = new Hashtable();
                GenerateAudioTable();
            }

            private void Dispose()
            {

            }

            private void GenerateAudioTable()
            {
                foreach(AudioTrack track in tracks)
                {
                    foreach(AudioObject Aobj in track.audio)
                    {
                        // non duplicare keys
                        if (AudioTable.ContainsKey(Aobj.type))
                        {
                            LogWarning("Stai cercando di registrare l'audio [" + Aobj.type + "] che è già stato registrato");
                        }
                        else
                        {
                            AudioTable.Add(Aobj.type, track);
                            Log("Registrazione audio in corso [" + Aobj.type + "].");
                        }
                    }
                }
            }

            private void Log(string _message)
            {
                if (!debug)
                {
                    return;
                }

                Debug.Log("[Audio Controller]: " + _message);
            }

            private void LogWarning(string _message)
            {
                if (!debug)
                {
                    return;
                }

                Debug.LogWarning("[Audio Controller]: " + _message);
            }


            private void AddFunction(AudioFunction _function)
            {
                // rimozione funzione conflittuale

               // RemoveConflictingFunctions(-_function.type);

                // avvio funzione
               // IEnumerator FunctionRunner = RunAudioFunction (_function)
            }

            #endregion




        }
    }
}

