using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class HandbookManager : MonoBehaviour
    {
        [SerializeField]
        private PadHandbookView padHandbookView; 
        [Space, SerializeField]
        private UnityEvent<int, int> onSubThemeButtonPressed;

        private int currentSubThemeNumber;
        private int selectedMainThemeNumber;
        private bool areMainThemeButtonsCreated = false;

        public void SetData(int currentSubThemeNumber)
        {
            this.currentSubThemeNumber = currentSubThemeNumber;
            if (areMainThemeButtonsCreated)
            {
                CreateMainThemeButtons();
                areMainThemeButtonsCreated = true;
            }
        }

        private void CreateMainThemeButtons()
        {
            var handbookMainThemes = GameContentManager.GetFirstCodingTrainingThemes(GameManager.CurrentLevelNumber)
                .Select(theme => theme.Title)
                .ToList();
            padHandbookView.CreateThemeButtons(TrainingThemeType.MainTheme, handbookMainThemes, CreateSubThemeButtons);
        }



        //public void OpenHandbook() => StartCoroutine(padHandbookView.OpenHandbook());

        //public void CloseHandbook() => StartCoroutine(CloseHandbook_COR());

        private void GoToSubThemeButtons(int mainThemeNumber) => StartCoroutine(GoToSubThemeButtons_COR(mainThemeNumber));

        private IEnumerator GoToSubThemeButtons_COR(int mainThemeNumber)
        {
            /*selectedMainThemeNumber = mainThemeNumber;
            var subThemes = GameContentManager.CodingTrainingInfos[selectedMainThemeNumber - 1]
                .Select(info => info.)
                .ToList();
            padHandbookView.CreateThemeButtons(TrainingThemeType.SubTheme, subThemes, ShowSubThemeContent);*/
            yield break;
        }


        private void CreateSubThemeButtons(int mainThemeNumber)
        {
            /*var subThemes = GameContentManager.CodingTrainingInfos[mainThemeNumber - 1]
                .Select(info => info.)
                .ToList();
            padHandbookView.CreateThemeButtons(TrainingThemeType.SubTheme, subThemes, ShowSubThemeContent);*/
        }

        private void ShowSubThemeContent(int subThemeNumber)
        {

        }
    }
}
