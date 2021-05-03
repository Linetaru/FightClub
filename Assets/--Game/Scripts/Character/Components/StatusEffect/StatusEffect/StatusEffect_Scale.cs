using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect_Scale : StatusEffect
{
    [SerializeField]
    private Vector3 newScale;

    private Vector3 originalScale;

    public StatusEffect_Scale()
    {
    }

    public StatusEffect_Scale(Vector3 scale)
    {
        newScale = scale;
    }


    public override StatusEffect Copy()
    {
        return new StatusEffect_Scale(newScale);
    }

    public override void ApplyEffect(CharacterBase character)
    {
        originalScale = character.transform.localScale;
        character.transform.localScale = newScale;

    }

    public override void UpdateEffect(CharacterBase character)
    {

    }

    public override void RemoveEffect(CharacterBase character)
    {
        character.transform.localScale = originalScale;
    }
}
