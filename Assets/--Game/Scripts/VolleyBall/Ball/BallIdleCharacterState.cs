using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BallIdleCharacterState : CharacterState
{
    [SerializeField]
    CharacterState ballBuntState;

    [SerializeField]
    CharacterState ballKickOffState;

    [SerializeField]
    CharacterState ballExplosionState;

    [SerializeField]
    GameObject trail;

    [Title("Parameter - Collision")]
    [SerializeField]
    float collisionFriction = 5f;
    [SerializeField]
    [Range(0, 1)]
    float reboundReduction = 0.2f;

    [SerializeField]
    float minimalHorizontalSpeed = 3f;

    [SerializeField]
    float minimalVerticalSpeed = -3f;


    private void Start()
    {
        
    }

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        character.Action.CancelAction();

        //Quand on frappe le ballon on repasse dans le start en changeant d'état


        //Si l'ancien état est soi-même, on continue d'appliquer la physique knockback

        character.Stats.LifePercentage = 80f;

        //if (oldState == this)
        //{
            character.Movement.SpeedX = character.Knockback.GetAngleKnockback().x;
            character.Movement.SpeedX *= character.Movement.Direction;
            character.Movement.SpeedY = character.Knockback.GetAngleKnockback().y;
        //}
        //else
        //{
        //    //Si l'ancien état n'est pas lui même, c'est que l'état précédent est le respawn donc on réinitialise
        //    isKickOff = true;
        //}

    }

    public override void UpdateState(CharacterBase character)
    {
        //if (character.Knockback.CanKnockback() && isKickOff)
        //    isKickOff = false;

        //if (!isKickOff)
        //{
            if (Mathf.Abs(character.Movement.SpeedX) > minimalHorizontalSpeed)
                character.Movement.SpeedX -= (collisionFriction * Mathf.Sign(character.Movement.SpeedX)) * Time.deltaTime;


            if (character.Movement.SpeedY > minimalVerticalSpeed)
                character.Movement.ApplyGravity(.6f);


            if (character.Movement.SpeedY < minimalVerticalSpeed)
                character.Movement.SpeedY += ((character.Movement.Gravity * .6f) * character.Movement.MotionSpeed) * Time.deltaTime;

            if (character.Stats.LifePercentage != 80f)
            {
                character.Stats.LifePercentage = 80f;
            }

        //}
        character.Knockback.UpdateKnockback(1);

        if (character.Knockback.KnockbackDuration > 0)
        {
            if(!trail.activeInHierarchy)
            trail.SetActive(true);
        }
        else
        {
            if(trail.activeInHierarchy)
            trail.SetActive(false);
        }


        if (character.Rigidbody.CollisionGroundInfo != null)
            character.SetState(ballExplosionState);
    }

    public override void LateUpdateState(CharacterBase character)
    {
        if ((character.Rigidbody.CollisionGroundInfo != null || character.Rigidbody.CollisionRoofInfo != null))
        {
            character.Movement.SpeedY = -character.Movement.SpeedY * reboundReduction;
        }

        if (character.Rigidbody.CollisionWallInfo.Collision != null)
        {
            character.Movement.SpeedX = -character.Movement.SpeedX * reboundReduction;
        }
    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }
}