using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{

    /// <summary>
    /// Copy comme son nom l'indique créer une copie de l'objet.
    /// C'est pour éviter de donner une référence au StatusUpdate du SO servant de modèle et de tout casser
    /// De plus on a besoin de plusieurs instances séparés quand l'effet est sur plusieurs personnes en même temps.
    /// </summary>
    public virtual StatusEffect Copy()
    {
        return new StatusEffect();
    }


    public virtual void ApplyEffect(CharacterBase character)
    {

    }

    public virtual void UpdateEffect(CharacterBase character)
    {

    }

    public virtual void RemoveEffect(CharacterBase character)
    {

    }
}
