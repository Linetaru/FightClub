using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionDetection : MonoBehaviour
{
    [SerializeField]
    CharacterBase character;
    [SerializeField]
    CharacterState stateKnockback;

    /*void Start()
    {
        
    }

    void Update()
    {
        
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
            return;

        if (character.Knockback.IsInvulnerable == true)
            return;
        AttackSubManager atkMan = other.GetComponent<AttackSubManager>();
        if (atkMan != null)
        {
            //Debug.Log(other.gameObject.name);

            character.Knockback.ContactPoint = (atkMan.HitBox.bounds.center + character.CenterPoint.position) / 2f;
            atkMan.Hit(character);

            if(character.Knockback.CanHit() == true)
                character.SetState(stateKnockback);
        }
    }
}
