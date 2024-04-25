using UnityEngine;

namespace Scripts
{
    public abstract class PadFunctionManager : MonoBehaviour
    {
        public abstract void Initialize(int currentTaskNumber);

        public abstract void ShowModalWindow();

        public abstract void HideModalWindow();
    }
}
