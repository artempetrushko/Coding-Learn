using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer audioMixer;

        public static void SetMusicVolume(int musicVolume)
        {
            SetAudioMixerParamValue("MusicVolume", GetVolumeInDb(musicVolume));
        }

        public static void SetSoundsVolume(int soundsVolume)
        {
            SetAudioMixerParamValue("SoundsVolume", GetVolumeInDb(soundsVolume));
        }

        private static void SetAudioMixerParamValue(string paramName, float paramValue) => audioMixer.SetFloat(paramName, paramValue);

        private static float GetVolumeInDb(int volume) => -40 + 40f / 100 * volume;
    }
}
