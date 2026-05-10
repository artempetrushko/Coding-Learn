using System;
using Cysharp.Threading.Tasks;

namespace MainMenu
{
	public interface IMainMenuSectionPresenter
    {
        event Action SectionDisabled;

        UniTask ShowSectionAsync();
        UniTask HideSectionAsync();
    }
}
