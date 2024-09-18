using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Sounds
{
    public class SoundsInstaller : MonoInstaller
    {
        [SerializeField] private AudioMixer _audioMixer;

        public override void InstallBindings()
        {
            Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle().NonLazy();
        }
    }
}
