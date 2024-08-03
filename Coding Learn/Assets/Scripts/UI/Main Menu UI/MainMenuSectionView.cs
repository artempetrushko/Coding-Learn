using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainMenuSectionView : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _buttonsContainer;

        [SerializeField]
        private List<Image> backgroundParts = new();
        [SerializeField]
        private Image blackScreen;

        public void SetContentLocalPosition(Vector3 localPosition) => _content.transform.localPosition = localPosition;

        
    }
}
