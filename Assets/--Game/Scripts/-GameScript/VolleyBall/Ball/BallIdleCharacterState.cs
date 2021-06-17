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

    bool hasTouchedScoreZone = false;
    [HideInInspector] public bool hasRedTeamScored = false;


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


    [Title("Ball Shadow")]
    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    SpriteRenderer ballShadowSprite;

    [SerializeField]
    AK.Wwise.Event eventBallRebound = null;

    RaycastHit hit;

    float maxShadowDistance = 8.0f;

    Color alphaColor;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        hasTouchedScoreZone = false;
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

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 50.0f, layerMask))
        {
            if (ballShadowSprite.gameObject.activeInHierarchy)
            {
                //float distance = (hit.point.y - transform.position.y) / 8;
                float newAlpha = 1 - ((transform.position.y - hit.point.y) / maxShadowDistance) + .25f;
                alphaColor = new Color(ballShadowSprite.color.r, ballShadowSprite.color.g, ballShadowSprite.color.b, newAlpha);
                ballShadowSprite.color = alphaColor;
                ballShadowSprite.transform.position = new Vector3(hit.point.x, hit.point.y + .15f, hit.point.z);
            }
            else
            {
                ballShadowSprite.gameObject.SetActive(true);
            }
        }
        else
        {
            if (ballShadowSprite.gameObject.activeInHierarchy)
            {
                //ballShadowSprite.gameObject.SetActive(false);
            }
        }
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
            if (!trail.activeInHierarchy)
                trail.SetActive(true);
        }
        else
        {
            if (trail.activeInHierarchy)
                trail.SetActive(false);
        }


        if (hasTouchedScoreZone)
            character.SetState(ballExplosionState);
    }

    public void RedTeamScored()
    {
        hasRedTeamScored = true;
        hasTouchedScoreZone = true;
    }

    public void BlueTeamScored()
    {
        hasRedTeamScored = false;
        hasTouchedScoreZone = true;
    }

    public override void LateUpdateState(CharacterBase character)
    {
        if ((character.Rigidbody.CollisionGroundInfo != null || character.Rigidbody.CollisionRoofInfo != null))
        {
            if(Mathf.Abs(character.Movement.SpeedY) > 1f)
                AkSoundEngine.PostEvent(eventBallRebound.Id, this.gameObject);
            character.Movement.SpeedY = -character.Movement.SpeedY * reboundReduction;
        }

        if (character.Rigidbody.CollisionWallInfo.Collision != null)
        {
            AkSoundEngine.PostEvent(eventBallRebound.Id, this.gameObject);
            character.Movement.SpeedX = -character.Movement.SpeedX * reboundReduction;
        }
    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }
}