using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_ChargingSmash : AttackComponent
{
    [SerializeField]
    AttackManager[] attackCharged;
    [SerializeField]
    float[] duration;

    [SerializeField]
    float initialDuration = 0.5f;
    [SerializeField]
    float autoRelease = 3f;

    [SerializeField]
    GameObject VFX;

    float timer = 0f;
    bool release = false;


    // Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
        timer = 0f;
        release = false;
    }

    // Appelé tant que l'attaque existe 
    //(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
    public override void UpdateComponent(CharacterBase user)
    {
        if(user.Input.CheckActionHold(InputConst.Special) && release == false)
        {
            timer += Time.deltaTime;
            if(timer >= autoRelease)
                release = true;
        }
        else if (release == false)
        {
            release = true;
        }



        if (release == true && timer < initialDuration)
        {
            timer += Time.deltaTime;
        }
        else if (release == true)
        {
            user.Action.CancelAction();
            for (int i = 0; i < duration.Length; i++)
            {
                if(timer < duration[i])
                {
                    user.Action.Action(attackCharged[i]);
                    return;
                }
            }
            user.Action.Action(attackCharged[attackCharged.Length-1]);
            return;
        }

    }


    // Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {

    }

    public override void OnParry(CharacterBase user, CharacterBase target)
    {

    }
    public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
    {

    }
    public override void OnClash(CharacterBase user, CharacterBase target)
    {

    }

    // Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {

    }
}
