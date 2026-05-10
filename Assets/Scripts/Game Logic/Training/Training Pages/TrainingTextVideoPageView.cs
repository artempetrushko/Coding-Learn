using UnityEngine;
using UnityEngine.Video;

namespace GameLogic
{
    public class TrainingTextVideoPageView : TrainingTextPageView
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        public void SetVideoClip(VideoClip clip) => _videoPlayer.clip = clip;
    }
}
