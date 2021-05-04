using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    [HideInInspector]
    public PackageCreator.Event.GameEventCharacters playerHitEvent;

    [Title("Parry Settings")]
    [SerializeField]
    private int clashLevel = 1;
    public int ClashLevel
    {
        get { return clashLevel; }
    }

    [SerializeField]
    private bool clashCancel = true;
    public bool ClashCancel
    {
        get { return clashCancel; }
    }

    [HorizontalGroup("Break")]
    [SerializeField]
    private bool parryBreak = false;
    public bool BreakParry
    {
        get { return parryBreak; }
    }
    [HorizontalGroup("Break")]
    [SerializeField]
    private bool guardBreak = false;
    public bool BreakGuard
    {
        get { return guardBreak; }
    }

    CharacterBase user;
    public CharacterBase User
    {
        get { return user; }
    }



    // Ces deux flags sont utilisés pour les collisions et gérer leur priorité (Clash prio sur Hit)
    /*bool hasHit;
    public bool HasHit
    {
        get { return hasHit; }
    }
    bool hasClash;
    public bool HasClash
    {
        get { return hasClash; }
    }*/

    private List<string> playerHitList = new List<string>();

    // Utilisé pour identifier l'attaque
    string attackID = "";
    public string AttackID
    {
        get { return attackID; }
    }



    [Button]
    public void UpdateComponents()
    {
        hitBox = GetComponent<Collider>();
        atkCompList = new List<AttackComponent>(GetComponentsInChildren<AttackComponent>());
    }

    [Title("Events")]
    [SerializeField]
    private UnityEvent<string> onHitColliderEvents;
    private bool eventReceived;

    // ===============================================================================


    public void Update()
    {
        foreach (AttackComponent atkC in atkCompList)
        {
            atkC.UpdateComponent(user);
        }
    }

    public void InitAttack(CharacterBase character, string attackName)
    {
        tag = character.tag;
        user = character;
        hitBox.enabled = false;
        attackID = attackName;

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

    public void AddPlayerHitList(string targetTag)
    {
        playerHitList.Add(targetTag);
    }

    public bool IsInHitList(string targetTag)
    {
        return playerHitList.Contains(targetTag);
    }





    public void Hit(CharacterBase target)
    {
        if(target.TeamID == TeamEnum.No_Team || target.TeamID != user.TeamID)
        {
            string targetTag = target.transform.root.tag;

            if (!playerHitList.Contains(targetTag))
            {
                playerHitList.Add(targetTag);
                if (onHitColliderEvents != null && !eventReceived)
                {
                    // Event pour eviter le multi hit
                    onHitColliderEvents.Invoke(targetTag);
                    // Event qui envoie le user et la target quand hit
                    if(playerHitEvent != null)
                        playerHitEvent.Raise(user, target);
                    eventReceived = true;
                }

                foreach (AttackComponent atkC in atkCompList)
                {
                    atkC.OnHit(user, target);
                }

                user.Action.HasHit(target);
            }
        }
    }







    AttackSubManager attackClashed;
    public AttackSubManager AttackClashed
    {
        get { return attackClashed; }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
            return;

        AttackSubManager atkMan = other.GetComponent<AttackSubManager>();
        if (atkMan != null)
        {
            if (atkMan.User.TeamID == user.TeamID) // Pour empêcher les joueurs dans la même équipes de clash
                return;

            attackClashed = atkMan;
            user.Knockback.ContactPoint = (atkMan.HitBox.bounds.center + user.CenterPoint.position) * 0.5f;
            atkMan.User.Knockback.RegisterHit(this);
        }
    }

}