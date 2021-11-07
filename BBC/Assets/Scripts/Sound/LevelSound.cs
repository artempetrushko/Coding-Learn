using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class LevelSound : MonoBehaviour
    {
        public AudioSource[] sounds, musics;
        public void Start()
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].volume = (float)PlayerPrefs.GetInt("soundvolume") / 10;
            }

            for (int i = 0; i < musics.Length; i++)
            {
                musics[i].volume = (float)PlayerPrefs.GetInt("musicvolume") / 10;
            }
        }
    }
}
