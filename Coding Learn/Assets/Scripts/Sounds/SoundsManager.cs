using UnityEngine.Audio;

namespace Scripts
{
    public class SoundsManager
    {
        private static AudioMixer _audioMixer;

        public SoundsManager(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
        }

        public static void SetMusicVolume(int musicVolume)
        {
            SetAudioMixerParamValue("MusicVolume", GetVolumeInDb(musicVolume));
        }

        public static void SetSoundsVolume(int soundsVolume)
        {
            SetAudioMixerParamValue("SoundsVolume", GetVolumeInDb(soundsVolume));
        }

        private static void SetAudioMixerParamValue(string paramName, float paramValue) => _audioMixer.SetFloat(paramName, paramValue);

        private static float GetVolumeInDb(int volume) => -40 + 40f / 100 * volume;
    }
}
