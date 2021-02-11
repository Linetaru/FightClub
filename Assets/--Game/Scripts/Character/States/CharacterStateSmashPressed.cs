using System.Collections;
using System.Collections.Generic;
using Feedbacks;
using UnityEngine;


public class CharacterStateSmashPressed : CharacterState
{
    [SerializeField]
    CharacterState idleState;
    [SerializeField]
    AttackManager attackSmash;

    [SerializeField]
    ParticleSystem chargingParticles;

    [SerializeField]
    Animator animator;

    private bool charging;


    [SerializeField]
    private float timeToReleaseSmash = 3.0f, timer = 0f;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        Debug.Log("Charging State");

        animator.Play("Michelle_Kick2Prep", 0, 0f);
        chargingParticles.Play();


        if (character.Input.inputActionsUP.Count != 0)
        {
            if (character.Input.inputActionsUP[0].action == InputConst.Attack)
            {
                character.Input.inputActionsUP[0].timeValue = 0;
            }
        }
        character.Movement.SpeedX = 0f;
        charging = true;
    }

    public override void UpdateState(CharacterBase character)
    {
        if (character.Input.inputActionsUP.Count != 0)
        {
            if (character.Input.inputActionsUP[0].action == InputConst.Attack)
            {
                character.Input.inputActionsUP[0].timeValue = 0;
                Attack(character);
            }
        }

        if (charging)
        {
            Charging(character);
        }
    }

    public override void EndState(CharacterBase character, CharacterState oldState)
    {

    }

    public void Charging(CharacterBase character)
    {
        timer += Time.deltaTime;
        Debug.Log(timer);
        if(timer >= timeToReleaseSmash)
        {
            Debug.Log("Smash Release");
            Attack(character);
        }
    }

    public void Attack(CharacterBase character)
    {
        Debug.Log("Attack Smash");
        
        chargingParticles.Stop();
        charging = false;
        timer = 0f;
        character.Action.Action(attackSmash);
    }

    void Start()
    {
        
    }
    void Update()
    {
    }
}
