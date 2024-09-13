using TMPro;
using UnityEngine;

namespace UI.MainMenu
{
    public class LevelsMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _header;
        [SerializeField] private TMP_Text _levelTitle;
        [SerializeField] private LevelThumbnailView _levelThumbnailView;
        [SerializeField] private LevelThumbnailView _transitionThumbnailView;
        [SerializeField] private GameObject _levelButtonsContainer;       
        [SerializeField] private LevelDescriptionView _levelDescriptionView;

        public GameObject Header => _header;
        public LevelThumbnailView LevelThumbnailView => _levelThumbnailView;
        public LevelThumbnailView TransitionThumbnailView => _transitionThumbnailView;
        public GameObject LevelButtonsContainer => _levelButtonsContainer;
        public LevelDescriptionView LevelDescriptionView => _levelDescriptionView;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetLevelTitleText(string text) => _levelTitle.text = text;
    }
}
