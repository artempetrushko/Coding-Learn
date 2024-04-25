using Cysharp.Threading.Tasks;

namespace Scripts
{
    public interface IMainMenuSectionController
    {
        UniTask ShowSectionViewAsync();
        UniTask HideSectionViewAsync();
    }
}
