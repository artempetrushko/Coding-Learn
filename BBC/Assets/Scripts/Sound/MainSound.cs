using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainSound : MonoBehaviour
    {
        public Slider soundSlider, musicSlider;
        public Text soundSliderText, musicSliderText;
        public AudioSource musicSource, soundSource;

        public void Start()
        {
            if (!PlayerPrefs.HasKey("musicvolume"))
                PlayerPrefs.SetInt("musicvolume", 5);

            if (!PlayerPrefs.HasKey("soundvolume"))
                PlayerPrefs.SetInt("soundvolume", 5);

            musicSlider.value = PlayerPrefs.GetInt("musicvolume");
            soundSlider.value = PlayerPrefs.GetInt("soundvolume");

            musicSource.volume = (float)PlayerPrefs.GetInt("musicvolume") / 10;
            soundSource.volume = (float)PlayerPrefs.GetInt("soundvolume") / 10;
        }

        public void Update()
        {
            PlayerPrefs.SetInt("musicvolume", (int)musicSlider.value);
            PlayerPrefs.SetInt("soundvolume", (int)soundSlider.value);

            soundSliderText.text = soundSlider.value.ToString();
            musicSliderText.text = musicSlider.value.ToString();

            musicSource.volume = (float)PlayerPrefs.GetInt("musicvolume") / 10;
            soundSource.volume = (float)PlayerPrefs.GetInt("soundvolume") / 10;
        }
    }
}
