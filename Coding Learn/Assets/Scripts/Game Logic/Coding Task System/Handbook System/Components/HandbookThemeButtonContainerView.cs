using UnityEngine;

namespace GameLogic
{
    public class HandbookThemeButtonContainerView : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonsContainer;

        public GameObject ButtonsContainer => _buttonsContainer;
    }
}
