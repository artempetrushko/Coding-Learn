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
        private Button buttonComponent;
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
        private LevelDescriptionView currentLevelDescriptionView;

        public void SetBasicParams(int levelNumber)
        {
            levelNumberLabel.text = levelNumber.ToString();
        }

        public void SetActiveButtonParams(string levelDescription, UnityAction buttonPressedAction)
        {           
            this.levelDescription = levelDescription;
            buttonComponent.onClick.AddListener(buttonPressedAction);
        }

        public void ClickForce() => buttonComponent.onClick.Invoke();

        public void Deactivate() => buttonComponent.interactable = false;

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

        private void ScaleButtonView(float addedScale) => buttonView.transform.localScale += new Vector3(addedScale, addedScale, addedScale);
    }
}
