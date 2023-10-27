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
    public class ChooseLevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        private GameObject buttonView;
        [SerializeField]
        private TMP_Text levelNumberLabel;
        [SerializeField]
        private Image innerArea;
        [Space]
        [SerializeField]
        private float pointerEnterButtonAddedScale = 0.5f;
        [SerializeField]
        private LevelDescriptionView levelDescriptionViewPrefab;
        [SerializeField]
        private Transform levelDescriptionViewContainer;

        private string levelDescription;
        private Color normalButtonColor;
        private Color selectedButtonColor;
        private Button buttonComponent;
        private LevelDescriptionView currentLevelDescriptionView;

        public void SetInfo(int levelNumber, string levelDescription)
        {
            levelNumberLabel.text = levelNumber.ToString();
            this.levelDescription = levelDescription;
        }

        public void SetButtonParams(bool isInteractable, Color normalButtonColor, Color selectedButtonColor, UnityAction buttonPressedAction)
        {
            buttonComponent.interactable = isInteractable;
            buttonComponent.onClick.AddListener(buttonPressedAction);
            this.normalButtonColor = normalButtonColor;
            this.selectedButtonColor = selectedButtonColor;
        }

        public void ClickForce() => buttonComponent.onClick.Invoke();

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

        public void OnSelect(BaseEventData eventData) => innerArea.color = selectedButtonColor;

        public void OnDeselect(BaseEventData eventData) => innerArea.color = normalButtonColor;

        private void OnEnable()
        {
            buttonComponent = GetComponent<Button>();
        }

        private void ScaleButtonView(float addedScale) => buttonView.transform.localScale += new Vector3(addedScale, addedScale, addedScale);
    }
}
