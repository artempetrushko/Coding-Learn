using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class ExitToMenuSection : MonoBehaviour
    {
        private Animator animator;

        public IEnumerator ShowContent_COR()
        {
            yield return StartCoroutine(PlayAnimation_COR("Show Exit To Menu Section"));
        }

        public IEnumerator HideContent_COR()
        {
            yield return StartCoroutine(PlayAnimation_COR("Show Exit To Menu Section"));
        }

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        private IEnumerator PlayAnimation_COR(string animationName)
        {
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
