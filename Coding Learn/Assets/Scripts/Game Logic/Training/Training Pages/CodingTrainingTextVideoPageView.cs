using UnityEngine;
using UnityEngine.Video;

namespace UI.Game
{
    public class CodingTrainingTextVideoPageView : TrainingTextPageView
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        public void SetVideoClip(VideoClip clip) => _videoPlayer.clip = clip;
    }
}
