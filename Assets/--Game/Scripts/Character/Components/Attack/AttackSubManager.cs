using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider))]
public class AttackSubManager : MonoBehaviour
{

    [SerializeField]
    private Collider hitBox;
    public Collider HitBox
    {
        get { return hitBox; }
    }

    [Title("Components")]
    [SerializeField]
    [ListDrawerSettings(Expanded = true)]
    private List<AttackComponent> atkCompList;


    CharacterBase user;
    private List<string> playerHitList = new List<string>();


    [Button]
    public void UpdateComponents()
    {
        hitBox = GetComponent<Collider>();
        atkCompList = new List<AttackComponent>(GetComponentsInChildren<AttackComponent>()); 
    }




    // ===============================================================================

    /*public void Start()
    {
        ActionActive();
    }*/

    public void Update()
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.UpdateComponent(user);
        }
    }

    public void InitAttack(CharacterBase character)
    {
        tag = character.tag;
        user = character;
        hitBox.enabled = false;

        for (int i = 0; i < atkCompList.Count; i++)
        {
            atkCompList[i].StartComponent(character);
        }
    }

    public void ActionActive()
    {
        playerHitList.Clear();
        hitBox.enabled = true;
    }

    public void ActionUnactive()
    {
        hitBox.enabled = false;
    }




    public void CancelAction()
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.EndComponent(user);
        }
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