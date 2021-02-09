using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private AnimationClip attackAnim;

    [SerializeField]
    private BoxCollider hitBox;

    [SerializeField]
    private List<AttackComponent> atkCompList;

    public void Start()
    {
        ActionActive();
    }

    public void ActionActive()
    {
        Debug.Log("Action Active");
        hitBox.enabled = true;
    }

    public void CancelAction()
    {
        Debug.Log("Action Canceled");
        hitBox.enabled = false;
    }

    public void Hit()
    {
        foreach(AttackComponent atkC in atkCompList)
        {
            atkC.OnHit();
        }
    }

}
