using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityCore
{
    namespace Audio
    {
        public class AudioInput : MonoBehaviour
        {
            public SoundManager soundManager;
            public GameController gameController;
            public MainPlayerScript mainPlayerScript;


            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    soundManager.PlayAudio(AudioType.ST_01);
                }

                if (mainPlayerScript._state == PlayerState.groundMoving)
                {
                    soundManager.PlayAudio(AudioType.SFX_PASSI_01);
                }
                else
                {
                    soundManager.StopAudio(AudioType.SFX_PASSI_01);
                }
            }
        }

    }
}
