using Cysharp.Threading.Tasks;

namespace Scripts
{
    public interface IMainMenuSectionManager
    {
        UniTask ShowSectionAsync();
        UniTask HideSectionAsync();
    }
}
