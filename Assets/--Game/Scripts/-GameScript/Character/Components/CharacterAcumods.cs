using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterAcumods : MonoBehaviour
{

    [SerializeField]
    int acumodSelected = 0;

    [Title("Acumods")]
    [SerializeField]
    int acumodAttackCost = 20;

    [SerializeField]
    StatusData statusAttack;
    public StatusData StatusAttack
    {
        get { return statusAttack; }
    }

    [SerializeField]
    CharacterState stateAttack;
    public CharacterState StateAttack
    {
        get { return stateAttack; }
    }



    [Space]
    [SerializeField]
    int acumodDefenseCost = 20;

    [SerializeField]
    StatusData statusDefense;
    public StatusData StatusDefense
    {
        get { return statusDefense; }
    }

    [SerializeField]
    CharacterState stateDefense;
    public CharacterState StateDefense
    {
        get { return stateDefense; }
    }



    [Space]
    [SerializeField]
    int acumodJumpCost = 20;

    [SerializeField]
    StatusData statusInfiniteJump;
    public StatusData StatusInfiniteJump
    {
        get { return statusInfiniteJump; }
    }

    [SerializeField]
    CharacterState stateInfiniteJump;
    public CharacterState StateInfiniteJump
    {
        get { return stateInfiniteJump; }
    }


    [Space]
    [SerializeField]
    int acumodBurstCost = 60;
    [SerializeField]
    CharacterState stateBurst;
    public CharacterState StateBurst
    {
        get { return stateBurst; }
    }


    public bool Acumod(CharacterBase character)
    {
        /*if (character.Input.CheckAction(0, InputConst.LeftShoulder))
        {
            character.Input.inputActions[0].timeValue = 0;
            switch(acumodSelected)
            {
                case 0: // Acumod Attack
                    return AcumodBuffAttack(character);
                case 1: // Acumod Defense
                    return AcumodBuffDefense(character);
                case 2: // Acumod Infinite Jump
                    return AcumodInfiniteJump(character);
                case 3: // Acumod Burst
                    return AcumodBurst(character);
            }

        }
        return false;*/
        return false;
    }

    private bool AcumodBuffAttack(CharacterBase character)
    {
        if (character.CurrentState is CharacterStateKnockback)
            return false;
        if (character.PowerGauge.CurrentPower >= acumodAttackCost)
        {
            character.PowerGauge.CurrentPower -= acumodAttackCost;
            character.Status.AddStatus(new Status("Acumod_Attack", statusAttack));
            character.SetState(stateAttack);
            return true;
        }
        return false;
    }

    private bool AcumodBuffDefense(CharacterBase character)
    {
        if (character.CurrentState is CharacterStateKnockback)
            return false;
        if (character.PowerGauge.CurrentPower >= acumodDefenseCost)
        {
            character.PowerGauge.CurrentPower -= acumodDefenseCost;
            character.Status.AddStatus(new Status("Acumod_Defense", statusDefense));
            character.SetState(stateDefense);
            return true;
        }
        return false;
    }

    private bool AcumodInfiniteJump(CharacterBase character)
    {
        if (character.CurrentState is CharacterStateKnockback)
            return false;
        if (character.PowerGauge.CurrentPower >= acumodJumpCost)
        {
            character.PowerGauge.CurrentPower -= acumodJumpCost;
            character.Status.AddStatus(new Status("Acumod_InfiniteJump", statusInfiniteJump));
            character.SetState(stateInfiniteJump);
            return true;
        }
        return false;
    }


    private bool AcumodBurst(CharacterBase character)
    {
        if (character.PowerGauge.CurrentPower >= acumodBurstCost)
        {
            character.PowerGauge.CurrentPower -= acumodBurstCost;
            character.SetState(stateBurst);
            return true;
        }
        return false;
    }
}
