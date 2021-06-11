using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public virtual void StartComponent(CharacterBase user)
    {

    }
    public virtual void UpdateComponent(CharacterBase user)
    {

    }
    public virtual void OnHit(CharacterBase user, CharacterBase target)
    {

    }

    public virtual void OnParry(CharacterBase user, CharacterBase target)
    {

    }
    public virtual void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
    {

    }
    public virtual void OnClash(CharacterBase user, CharacterBase target)
    {

    }

    public virtual void EndComponent(CharacterBase user)
    {

    }
}