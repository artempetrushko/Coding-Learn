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

        public void SetMusicVolume(int musicVolume)
        {
            var musicVolumeInDb = -40 + 40f / 100 * musicVolume;
            SetAudioMixerParamValue("MusicVolume", musicVolumeInDb);
        }

        public void SetSoundsVolume(int soundsVolume)
        {
            var soundsVolumeInDb = -40 + 40f / 100 * soundsVolume;
            SetAudioMixerParamValue("SoundsVolume", soundsVolumeInDb);
        }

        private void SetAudioMixerParamValue(string paramName, float paramValue) => audioMixer.SetFloat(paramName, paramValue);
    }
}
