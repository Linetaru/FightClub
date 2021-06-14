using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_SmearsSword : AttackComponent
{
    [SerializeField]
    List<Collider> colliders = new List<Collider>();

    SmearsControllerSword smearsController;

    // Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {
        // Yikes
        smearsController = user.SmearsControllerSword;
	}


    public override void UpdateComponent(CharacterBase user)
    {
        if (smearsController == null)
            return;

        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].enabled)
            {
                smearsController.SmearsActive(true);
                return;
            }

        }
        smearsController.SmearsActive(false);
    }

    // Appelé au moment où l'attaque touche une target
    /*public override void OnHit(CharacterBase user, CharacterBase target)
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

    }*/

    // Appelé au moment de la destruction de l'attaque
    public override void EndComponent(CharacterBase user)
    {
        if (smearsController != null)
            smearsController.SmearsActive(false);
    }
}
