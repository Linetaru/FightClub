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


    CharacterBase user;
    public CharacterBase User
    {
        get { return user; }
    }

    bool hasClash;
    public bool HasClash
    {
        get { return hasClash; }
    }

    private List<string> playerHitList = new List<string>();


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
        string targetTag = target.transform.root.tag;

        if (!playerHitList.Contains(targetTag))
        {
            playerHitList.Add(targetTag);
            if (onHitColliderEvents != null && !eventReceived)
            {
                //Debug.Log("Allo");
                onHitColliderEvents.Invoke(targetTag);
                eventReceived = true;
            }
            /*else
            {
                playerHitList.Add(targetTag);
            }*/

            foreach (AttackComponent atkC in atkCompList)
            {
                atkC.OnHit(user, target);
            }
            user.Action.HasHit(target);
        }
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
            return;
        AttackSubManager atkMan = other.GetComponent<AttackSubManager>();
        if (atkMan != null)
        {
            Debug.Log("Clash");
            hasClash = true;
        }
    }




}