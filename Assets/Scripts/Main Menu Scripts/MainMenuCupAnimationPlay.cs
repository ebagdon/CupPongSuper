using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCupAnimationPlay : MonoBehaviour
{
    // animator component
    private Animator animator;

    private void Awake()
    {
        // get the animator
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {   
        // see if we collided with the ball
        if (collision.CompareTag(Tags.PLAYERS_BALL_TAG))
        {
            // disable the ball and start our animation
            collision.gameObject.SetActive(false);
            animator.Play(AnimationNames.MAIN_MENU_CUP_ANIMATION_NAME);
        }
    }
}