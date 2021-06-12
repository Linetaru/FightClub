using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Les characters sont composés de plusieurs collider donc on a un script qui se charge de tout les colliders
public class CharacterCollisionDetection : MonoBehaviour
{
    [SerializeField]
    CharacterBase character;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
            return;
        if (character.Knockback.IsInvulnerable == true)
            return;

        AttackSubManager atkMan = other.GetComponent<AttackSubManager>();
        if (atkMan != null)
        {
            if (atkMan.IsInHitList(this.tag) == false)
            {
                character.Knockback.ContactPoint = (atkMan.HitBox.bounds.center + character.CenterPoint.position) / 2f;

                // Register Collision
                character.Knockback.RegisterHit(atkMan);
            }
        }
    }
}
