using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackManager : MonoBehaviour
{
    /*[SerializeField]
    private CharacterState attackState;
    public CharacterState AttackState
    {
        get { return attackState; }
    }*/


    [SerializeField]
    private AnimationClip attackAnim;
    public AnimationClip AttackAnim
    {
        get { return attackAnim; }
    }


    [SerializeField]
    private BoxCollider hitBox;
    public BoxCollider HitBox
    {
        get { return hitBox; }
    }

    [Title("Parameters")]
    [SerializeField]
    bool activeAtStart = true;
    [SerializeField]
    bool linkToCharacter = true;
    [SerializeField]
    bool noDirection = false; // à virer

    [SerializeField]
    private AttackManager atkCombo;
    public AttackManager AtkCombo
    {
        get { return atkCombo; }
    }

    [Title("Multiple Hitbox")]
    [SerializeField]
    [ListDrawerSettings(Expanded = true)]
    private List<AttackManager> atkSubs;


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

    public void Update()
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.UpdateComponent(user);
        }
    }

    public void CreateAttack(CharacterBase character)
    {
        tag = character.tag;
        user = character;
        if (noDirection == false)
        {
            transform.localScale = new Vector3(transform.localScale.x * character.transform.localScale.x * user.Movement.Direction,
                                               transform.localScale.y * character.transform.localScale.y,
                                               transform.localScale.z * character.transform.localScale.z);
        }
        hitBox.enabled = false;
        gameObject.SetActive(false);
        if (linkToCharacter == true)
            this.transform.SetParent(user.transform);

        for (int i = 0; i < atkCompList.Count; i++)
        {
            atkCompList[i].StartComponent(character);
        }

        for (int i = 0; i < atkSubs.Count; i++)
        {
            atkSubs[i].CreateAttack(character);
        }
    }

    public void ActionActive(int subAttack = 0)
    {
        if (subAttack == 0)
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
        else
        {
            atkSubs[subAttack - 1].ActionActive();
        }
    }

    public void ActionUnactive(int subAttack = 0)
    {
        if (subAttack == 0)
        {
            hitBox.enabled = false;
        }
        else
        {
            atkSubs[subAttack - 1].ActionActive();
        }
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

}