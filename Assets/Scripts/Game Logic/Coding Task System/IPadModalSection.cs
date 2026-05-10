using Cysharp.Threading.Tasks;

namespace GameLogic
{
    public interface IPadModalSection
    {
        UniTask ShowModalSectionAsync();
        UniTask HideModalSectionAsync();
    }
}
