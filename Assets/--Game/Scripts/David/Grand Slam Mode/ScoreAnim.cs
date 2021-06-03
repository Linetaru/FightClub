using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnim : MonoBehaviour
{
    private Animator animator;

    public void TriggerAnim()
    {
        if(animator == null)
            animator = GetComponent<Animator>();

        if(!animator.enabled)
            animator.enabled = true;

        animator.SetBool("isPulsating", true);
    }

    public void StopAnim()
    {
        animator.SetBool("isPulsating", false);
    }
}
