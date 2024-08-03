using UnityEngine;
using UnityEngine.Video;

namespace Scripts
{
    public class CodingTrainingTextVideoPageView : CodingTrainingTextPageView
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        public void SetVideoClip(VideoClip clip) => _videoPlayer.clip = clip;
    }
}
