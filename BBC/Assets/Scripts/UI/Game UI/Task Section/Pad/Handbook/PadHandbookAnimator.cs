using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class PadHandbookAnimator : MonoBehaviour
    {
        public void ShowHandbook()
        {
            transform.DOScale(Vector3.one, 1.5f);
        }
    }
}
