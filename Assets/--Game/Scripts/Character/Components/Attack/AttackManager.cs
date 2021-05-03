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






    [Title("Parameters")]
    [SerializeField]
    bool linkToCharacter = true;

    [SerializeField]
    private AttackManager atkCombo;
    public AttackManager AtkCombo
    {
        get { return atkCombo; }
    }

    [SerializeField]
    private CharacterConditionGameObject attackCondition;

    [SerializeField]
    private PackageCreator.Event.GameEventCharacters playerHitEvent;

    [Title("Multiple Hitbox")]
    [SerializeField]
    [ListDrawerSettings(Expanded = true)]
    private List<AttackSubManager> atkSubs;


    CharacterBase user;
    private List<string> playerHitList = new List<string>();



    [Button]
    public void UpdateComponents()
    {
        atkSubs = new List<AttackSubManager>(GetComponentsInChildren<AttackSubManager>()); 
    }




    // ===============================================================================

    /*public void Start()
    {
        ActionActive();
    }*/

   /* public void Update()
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.UpdateComponent(user);
        }
    }*/

    public bool CanUseAttack(CharacterBase character)
    {
        if (attackCondition == null)
            return true;
        return attackCondition.CheckConditions(character);
    }

    public void CreateAttack(CharacterBase character)
    {
        tag = character.tag;
        user = character;
        transform.localScale = new Vector3(transform.localScale.x * character.transform.localScale.x * user.Movement.Direction,
                                           transform.localScale.y * character.transform.localScale.y,
                                           transform.localScale.z * character.transform.localScale.z);
        if (linkToCharacter == true)
            this.transform.SetParent(user.transform);

        for (int i = 0; i < atkSubs.Count; i++)
        {
            atkSubs[i].playerHitEvent = playerHitEvent;
            atkSubs[i].InitAttack(character);
        }
    }

    public void ActionActive(int subAttack = 0)
    {
        atkSubs[subAttack].ActionActive();
    }

    public void ActionUnactive(int subAttack = 0)
    {
        atkSubs[subAttack].ActionUnactive();
    }

    public void ActionAllActive()
    {
        foreach (AttackSubManager atkSub in atkSubs)
        {
            atkSub.ActionActive();
        }
    }

    public void ActionAllUnactive()
    {
        foreach (AttackSubManager atkSub in atkSubs)
        {
            atkSub.ActionUnactive();
        }
    }
    public void AddPlayerHitList(string targetTag)
    {
        foreach (AttackSubManager atkSub in atkSubs)
        {
            atkSub.AddPlayerHitList(targetTag);
        }
    }

    /*
    public void ActionAllUnactiveExceptOne(AttackSubManager atkStayActive)
    {
        foreach (AttackSubManager atkSub in atkSubs)
        {
            if(atkSub != atkStayActive)
            {
                atkSub.ActionUnactive();
            }
        }
    }
    */

    public void CancelAction()
    {
        EndAction();
    }

    public void EndAction()
    {
        for (int i = 0; i < atkSubs.Count; i++)
        {
            atkSubs[i].CancelAction();
        }
        Destroy(this.gameObject);
    }

    /*
    public void Hit(CharacterBase target)
    {
        user.Action.HasHit(target);

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
    */

}