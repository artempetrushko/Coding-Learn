using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Scripts
{
    public class CodingTrainingTextVideoPageView : CodingTrainingTextPageView
    {
        [SerializeField]
        private VideoPlayer videoPlayer;

        public void SetContent(string trainingContent, VideoClip trainingVideo)
        {
            SetContent(trainingContent);
            videoPlayer.clip = trainingVideo;
        }
    }
}
