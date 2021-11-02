using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveEnvironment : MonoBehaviour
{
    [SerializeField] private string openAnimationName;
    [SerializeField] private string closeAnimationName;

    private GameManager gameManager;
    private bool isPlayerClose;
    private bool isAnimationStarted = false;
    private bool isOpenAnimation = true;

    private IEnumerator PlayAnimation_COR(string animationName)
    {
        isAnimationStarted = true;
        var animator = GetComponentInParent<Animator>();
        animator.Play(animationName);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAnimationStarted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameManager.Player)
        {
            GetComponent<InteractiveItemMarker>().enabled = true;
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == gameManager.Player)
        {
            GetComponent<InteractiveItemMarker>().enabled = false;
            isPlayerClose = false;
        }
    }

    private void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E) && !isAnimationStarted)
        {
            if (isOpenAnimation)
                StartCoroutine(PlayAnimation_COR(openAnimationName));
            else StartCoroutine(PlayAnimation_COR(closeAnimationName));
            isOpenAnimation = !isOpenAnimation;
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
}
