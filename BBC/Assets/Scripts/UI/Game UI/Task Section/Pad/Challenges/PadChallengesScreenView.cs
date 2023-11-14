using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class PadChallengesScreenView : MonoBehaviour
    {
        [Tooltip("Панель испытаний")]
        public GameObject ChallengesPanel;
        [Tooltip("Префаб испытания (UI)")]
        public GameObject ChallengePrefab;

        /*private void LoadNewChallenges()
        {
            var challengesHolder = ChallengesPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;
            for (var i = challengesHolder.transform.childCount; i > 0; i--)
                Destroy(challengesHolder.transform.GetChild(i - 1).gameObject);
            var challengeTexts = ResourcesData.TaskChallenges[gameManager.SceneIndex - 1][gameManager.GetCurrentTaskNumber() - 1];
            foreach (var challengeText in challengeTexts)
            {
                var challenge = Instantiate(ChallengePrefab, challengesHolder);
                challenge.GetComponentInChildren<TMP_Text>().text = challengeText.Challenge;
            }
        }
        
        public void OpenChallengesPanel() => StartCoroutine(PlayAnimation_COR(ChallengesPanel, "ScaleUp"));

        public void CloseChallengesPanel() => StartCoroutine(PlayAnimation_COR(ChallengesPanel, "ScaleDown")); 

        private IEnumerator PlayAnimation_COR(GameObject animatorHolder, string animationName)
        {
            var animator = animatorHolder.GetComponent<Animator>();
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }

         */
    }
}
