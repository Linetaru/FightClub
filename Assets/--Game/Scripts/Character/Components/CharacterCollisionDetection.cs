using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDetection : MonoBehaviour
{
    [SerializeField]
    CharacterBase character;
    [SerializeField]
    CharacterState stateKnockback;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
            return;

        if (character.Knockback.IsInvulnerable == true)
            return;
        if (other.GetComponent<AttackManager>() != null)
        {
            AttackManager atkMan = other.GetComponent<AttackManager>();

            character.Knockback.ContactPoint = atkMan.HitBox.bounds.center;
            atkMan.Hit(character);

            character.SetState(stateKnockback);
        }
    }
}
