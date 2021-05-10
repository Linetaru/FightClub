using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponent : MonoBehaviour
{
    public abstract void StartComponent(CharacterBase user);
    public abstract void UpdateComponent(CharacterBase user);
    public abstract void OnHit(CharacterBase user, CharacterBase target);

    public abstract void OnParry(CharacterBase user, CharacterBase target);
    public abstract void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel);
    public abstract void OnClash(CharacterBase user, CharacterBase target);

    public abstract void EndComponent(CharacterBase user);
}