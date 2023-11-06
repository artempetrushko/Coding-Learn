using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ChallengeView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text challengeDescriptionText;
        [SerializeField]
        private Image starImage;

        private Animator animator;

        public void SetChallengeDescription(string challengeDescription) => challengeDescriptionText.text = challengeDescription;

        public IEnumerator PlayChallengeCompletedAnimation_COR()
        {
            yield return StartCoroutine(PlayAnimation_COR(""));
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
