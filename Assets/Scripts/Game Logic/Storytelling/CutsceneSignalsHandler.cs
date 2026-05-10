using System;
using UnityEngine;

namespace GameLogic
{
    public class CutsceneSignalsHandler : MonoBehaviour
    {
        public event Action ShowStorySignalReceived;
        public event Action StopCurrentFrameSignalReceived;

        public void ShowStory() => ShowStorySignalReceived?.Invoke();

        public void StopCurrentFrame() => StopCurrentFrameSignalReceived?.Invoke();
    }
}
