using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_JoystickMovement : AttackComponent
{
    public float joystickDeadZone = .3f;



    Vector2 direction;

    // Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
        direction = new Vector2(user.Input.horizontal, user.Input.vertical);

        direction.Normalize();
        Debug.Log(direction);

        if (Mathf.Abs(user.Input.horizontal) < joystickDeadZone && Mathf.Abs(user.Input.vertical) < joystickDeadZone)
        {
            Debug.Log(direction);
            user.Movement.SetSpeed(0, user.Movement.SpeedMax);
            return;
        }
        //if (direction.x != 0)
        //    user.Movement.Direction = (int)direction.x;
        user.Movement.SetSpeed(user.Movement.Direction * direction.x * user.Movement.SpeedMax, direction.y * user.Movement.SpeedMax);
    }

    // Appelé tant que l'attaque existe 
    //(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
    public override void UpdateComponent(CharacterBase user)
    {

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
