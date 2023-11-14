using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class ExitToMenuManager : MonoBehaviour
    {
        [SerializeField]
        private ExitToMenuSectionView exitToMenuSectionView;

        public void ExitToMenu()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

            }
        }

        private IEnumerator ExitToMenu_COR()
        {
            yield return StartCoroutine(exitToMenuSectionView.HideContent_COR());
        }
    }
}
