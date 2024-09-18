using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public event Action<LevelButton> PointerEnter;
        public event Action<LevelButton> PointerExit;
        public event Action<LevelButton> ButtonSelected;
        public event Action<LevelButton> ButtonDeselected;

        [SerializeField] private Button _buttonComponent;
        [SerializeField] private TMP_Text _levelNumberLabel;
        [SerializeField] private Image _innerArea;

        public Button ButtonComponent => _buttonComponent;

        public void SetInteractable(bool isInteractable) => _buttonComponent.interactable = isInteractable;

        public void SetLevelNumberLabelText(string text) => _levelNumberLabel.text = text;

        public void SetInnerAreaColor(Color color) => _innerArea.color = color;

        public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke(this);

        public void OnPointerExit(PointerEventData eventData) => PointerExit?.Invoke(this);

        public void OnSelect(BaseEventData eventData) => ButtonSelected?.Invoke(this);

        public void OnDeselect(BaseEventData eventData) => ButtonDeselected?.Invoke(this);
    }
}
