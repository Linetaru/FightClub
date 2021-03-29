using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_JoystickMovement : AttackComponent
{
    public float joystickDeadZone = .3f;
    public float speedMultiplier = 1f;

    public bool canDeccelerate = false;

    [ShowIf("canDeccelerate")]
    [HideLabel]
    [HorizontalGroup("")]
    public AnimationCurve deccelerationCurve;
    [ShowIf("canDeccelerate")]
    [HorizontalGroup("")]
    public float timeSpeed;

    Vector2 direction;
    float timer = 0;

    // Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
        timer = 0f;
        timeSpeed /= 60f;

        direction = new Vector2(user.Input.horizontal, user.Input.vertical);
        direction.Normalize();

        if (Mathf.Abs(user.Input.horizontal) < joystickDeadZone && Mathf.Abs(user.Input.vertical) < joystickDeadZone)
        {
            direction.y = 1f;
            user.Movement.SetSpeed(0, user.Movement.SpeedMax * speedMultiplier);
            return;
        }

        if (direction.x != 0)
            user.Movement.Direction = (int)Mathf.Sign(direction.x);
        user.Movement.SetSpeed(user.Movement.Direction * direction.x * user.Movement.SpeedMax * speedMultiplier, direction.y * user.Movement.SpeedMax * speedMultiplier);
    }

    // Appelé tant que l'attaque existe 
    public override void UpdateComponent(CharacterBase user)
    {
        if(canDeccelerate == true)
        {
            timer += Time.deltaTime * user.MotionSpeed;
            float multiplier = deccelerationCurve.Evaluate(timer / timeSpeed);
            user.Movement.SetSpeed(user.Movement.Direction * direction.x * user.Movement.SpeedMax * speedMultiplier * multiplier, direction.y * user.Movement.SpeedMax * speedMultiplier * multiplier);
        }
    }

    // Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {

    }

    // Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {

    }
}
