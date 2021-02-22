﻿using System.Collections;
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

    CharacterBase character;

    public override void StartComponent(CharacterBase user)
    {
        if (keepMomentum == false)
            user.Movement.SetSpeed(0, 0);
        if (linkToCharacter == true)
            this.transform.SetParent(user.transform);
        character = user;
    }

    // Jsp
    private void Update()
    {
        if (applyGravity == true)
            character.Movement.ApplyGravity();
        if (deccelerate == true)
            character.Movement.Decelerate();
    }

    // jsp
    public override void UpdateComponent(CharacterBase user)
    {
        if (applyGravity == false)
            user.Movement.ApplyGravity();
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