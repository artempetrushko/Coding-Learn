using UnityEngine;
using Zenject;

namespace Scripts
{
    public class SettingsControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private SettingsSectionView settingsSectionView;
        [SerializeField]
        private AudioManager audioManager;

        public override void InstallBindings()
        {
            Container.Bind<SettingsSectionView>().FromInstance(settingsSectionView).AsSingle().NonLazy();
            Container.Bind<AudioManager>().FromInstance(audioManager).AsSingle().NonLazy();
        }
    }
}