﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateLanding : CharacterState
{
    [Title("States")]
    [SerializeField]
    CharacterState idleState;

    [Title("Parameter")]
    [SerializeField]
    [SuffixLabel("en frames")]
    float landingTime = 10f;

    [Title("Particle")] // à dégager un jour
    [SerializeField]
    ParticleSystem landParticleSystem;

    float maxLandingTime = 0f;
    float currentLandingTime = 0f;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        maxLandingTime = landingTime / 60f;
        currentLandingTime = 0f;

        ParticleSystem particle = Instantiate(landParticleSystem, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        Destroy(particle.gameObject, 0.5f);
    }

    public override void UpdateState(CharacterBase character)
    {
        currentLandingTime += Time.deltaTime;
        if (currentLandingTime >= maxLandingTime)
        {
            character.SetState(idleState);
        }
    }

    /*public override void LateUpdateState(CharacterBase character)
    {

    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }*/
}