using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        private GameObject buttonView;
        [SerializeField]
        private TMP_Text levelNumberLabel;
        [SerializeField]
        private Image innerArea;
        [Space, SerializeField]
        private float pointerEnterButtonAddedScale = 0.5f;
        [Space, SerializeField]
        private Color buttonNormalColor;
        [SerializeField]
        private Color buttonSelectedColor;
        [Space, SerializeField]
        private LevelDescriptionView levelDescriptionViewPrefab;
        [SerializeField]
        private Transform levelDescriptionViewContainer;

        private string levelDescription;
        private Button buttonComponent;
        private LevelDescriptionView currentLevelDescriptionView;

        public void SetInfo(int levelNumber, string levelDescription)
        {
            levelNumberLabel.text = levelNumber.ToString();
            this.levelDescription = levelDescription;
        }

        public void SetButtonParams(bool isInteractable, UnityAction buttonPressedAction)
        {
            buttonComponent.interactable = isInteractable;
            buttonComponent.onClick.AddListener(buttonPressedAction);
        }

        public void Select() => buttonComponent.Select();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonComponent.interactable)
            {
                ScaleButtonView(pointerEnterButtonAddedScale);
                var levelDescriptionView = Instantiate(levelDescriptionViewPrefab, levelDescriptionViewContainer);
                levelDescriptionView.SetInfo(levelDescription);
                currentLevelDescriptionView = levelDescriptionView;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonComponent.interactable)
            {
                ScaleButtonView(-pointerEnterButtonAddedScale);
                if (currentLevelDescriptionView != null)
                {
                    Destroy(currentLevelDescriptionView.gameObject);
                }
            }
        }

        public void OnSelect(BaseEventData eventData) => innerArea.color = buttonNormalColor;

        public void OnDeselect(BaseEventData eventData) => innerArea.color = buttonSelectedColor;

        private void OnEnable()
        {
            buttonComponent = GetComponent<Button>();
        }

        private void ScaleButtonView(float addedScale) => buttonView.transform.localScale += new Vector3(addedScale, addedScale, addedScale);
    }
}
