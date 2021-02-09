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

    private List<string> playerHitList = new List<string>();

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

    public void EndAction()
    {
        Destroy(this.gameObject);
    }

    public void Hit(CharacterKnockback target)
    {
        string targetTag = target.transform.root.tag;

        if(!playerHitList.Contains(targetTag))
        {
            foreach (AttackComponent atkC in atkCompList)
            {
                atkC.OnHit(target);
            }
        }

        playerHitList.Add(targetTag);
    }

}