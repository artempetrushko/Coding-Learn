using UnityEngine;
using Zenject;

namespace Scripts
{
    public class TipsControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private PadTipsScreenView padTipsScreenView;
        [Space, SerializeField]
        private TipSectionLabelsData tipStatusLabelsData;
        [SerializeField]
        private TipSectionLabelsData taskSkippingStatusLabelsData;
        [SerializeField]
        private int timeToNextTipInSeconds;
        [SerializeField]
        private int timeToSkipTaskInSeconds;

        public override void InstallBindings()
        {
            Container.Bind<PadTipsScreenView>().FromInstance(padTipsScreenView).AsSingle().NonLazy();
            Container.Bind<TipsController>().ToSelf().AsSingle().WithArguments(tipStatusLabelsData, taskSkippingStatusLabelsData, timeToNextTipInSeconds, timeToSkipTaskInSeconds).NonLazy();
        }
    }
}