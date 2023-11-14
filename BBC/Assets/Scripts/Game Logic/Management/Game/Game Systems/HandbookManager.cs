using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Scripts
{
    public class HandbookManager : MonoBehaviour
    {
        [SerializeField]
        private PadHandbookView padHandbookView;
        [Space, SerializeField]
        private int availableThemesCount;     

        [Space]
        [SerializeField] private UnityEvent<int, int> onSubThemeButtonPressed;

        private int currentSubThemeNumber;

        public void CreateMainThemeButtons(List<ThemeTitle> mainThemes)
        {
            //padHandbookView.CreateThemeButtons(TrainingThemeType.MainTheme, mainThemes, );
        }

        public void SetData(int currentSubThemeNumber)
        {
            this.currentSubThemeNumber = currentSubThemeNumber;
        }

        /*public void OpenHandbook() => StartCoroutine(OpenHandbook_COR());

        public void CloseHandbook() => StartCoroutine(CloseHandbook_COR());

        public void OpenSubTheme(int subThemeNumber)
        {
            //uiManager.PadMode = PadMode.HandbookProgrammingInfo;
            onSubThemeButtonPressed.Invoke(currentThemeNumber, subThemeNumber);
        }*/
    }
}
