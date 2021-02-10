using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackManager : MonoBehaviour
{
    [SerializeField]
    private AnimationClip attackAnim;
    public AnimationClip AttackAnim
    {
        get { return attackAnim; }
    }


    [SerializeField]
    private BoxCollider hitBox;

    [Title("Parameters")]
    [SerializeField]
    bool activeAtStart = true;


    [Title("Components")]
    [SerializeField]
    [ListDrawerSettings(Expanded = true)]
    private List<AttackComponent> atkCompList;


    CharacterBase user;
    private List<string> playerHitList = new List<string>();
    bool firstTime = false;




    [Button]
    public void UpdateComponents()
    {
        atkCompList = new List<AttackComponent>(GetComponentsInChildren<AttackComponent>()); 
    }




    // ===============================================================================

    public void Start()
    {
        ActionActive();
    }

    public void CreateAttack(CharacterBase character)
    {
        user = character;

        hitBox.enabled = false;
        gameObject.SetActive(false);

        for (int i = 0; i < atkCompList.Count; i++)
        {
            atkCompList[i].StartComponent(character);
        }
    }

    public void ActionActive()
    {
        if (firstTime == false)
        {
            gameObject.SetActive(true);
            firstTime = true;
            if (activeAtStart == false)
                return;
        }
        hitBox.enabled = true;
    }

    public void ActionUnactive()
    {
        hitBox.enabled = false;
    }




    public void CancelAction()
    {
        EndAction();
    }

    public void EndAction()
    {
        Destroy(this.gameObject);
    }




    public void Hit(CharacterBase target)
    {
        string targetTag = target.transform.root.tag;

        if(!playerHitList.Contains(targetTag))
        {
            foreach (AttackComponent atkC in atkCompList)
            {
                atkC.OnHit(user, target);
            }
        }

        playerHitList.Add(targetTag);
    }

}