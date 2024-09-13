using Cysharp.Threading.Tasks;

namespace UI.Game
{
    public interface IPadModalSection
    {
        UniTask ShowModalSectionAsync();
        UniTask HideModalSectionAsync();
    }
}
