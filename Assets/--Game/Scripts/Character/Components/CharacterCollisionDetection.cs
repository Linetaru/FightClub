using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Les characters sont composés de plusieurs collider donc on a un script qui se charge de tout les colliders
public class CharacterCollisionDetection : MonoBehaviour
{
    [SerializeField]
    CharacterBase character;
    /*[SerializeField]
    CharacterState stateKnockback;*/


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
            return;
        if (character.Knockback.IsInvulnerable == true)
            return;

        AttackSubManager atkMan = other.GetComponent<AttackSubManager>();
        if (atkMan != null)
        {
            character.Knockback.ContactPoint = (atkMan.HitBox.bounds.center + character.CenterPoint.position) / 2f;

            // Register Collision
            character.Knockback.RegisterHit(atkMan);


            /* if (character.Parry.CanParry(atkMan) == true)   // On parry
             {
                 Debug.Log("Parry");
                 character.Parry.Parry(character, atkMan.User);
                 atkMan.User.Parry.ParryRepel(atkMan.User, character);
                 atkMan.AddPlayerHitList(character.tag);
             }
             else if (!atkMan.IsInHitList(character.tag))  // On se prend l'attaque
             {
                 atkMan.Hit(character);
                 if (character.Knockback.CanKnockback() == true)
                     character.SetState(stateKnockback);
             }*/
        }
    }
}
