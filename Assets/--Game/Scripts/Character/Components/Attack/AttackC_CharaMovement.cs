using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


// Ptet a bouger pour les states machines pour chaque action ?
public class AttackC_CharaMovement : AttackComponent
{

    [HorizontalGroup("Movement")]
    [SerializeField]
    bool linkToCharacter = true;

    [HorizontalGroup("Movement")]
    [SerializeField]
    bool keepMomentum = false;

    [ShowIf("keepMomentum")]
    [SerializeField]
    bool applyGravity = false;
    [ShowIf("keepMomentum")]
    [SerializeField]
    bool deccelerate = false;

    [SerializeField]
    bool groundCancel = false;

    [SerializeField]
    float gravityMultiplier = 1f;

    CharacterBase character;

    public override void StartComponent(CharacterBase user)
    {
        if (keepMomentum == false)
            user.Movement.SpeedX = 0;
        if (linkToCharacter == true)
            this.transform.SetParent(user.transform);
        character = user;
    }

    // Jsp
    private void Update()
    {
        if (applyGravity == true)
            character.Movement.ApplyGravity(gravityMultiplier);
        if (deccelerate == true)
            character.Movement.Decelerate();

        if (groundCancel == true && character.Rigidbody.CollisionGroundInfo != null)
            character.Action.EndAction();
    }

    // jsp
    public override void UpdateComponent(CharacterBase user)
    {

        if (applyGravity == false)
            user.Movement.ApplyGravity(gravityMultiplier);
        if (deccelerate == true)
            user.Movement.Decelerate();
    }
    public override void OnHit(CharacterBase user, CharacterBase target)
    {

    }
    public override void EndComponent(CharacterBase user)
    {

    }
}
