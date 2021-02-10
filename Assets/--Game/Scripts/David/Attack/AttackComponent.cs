using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackComponent : MonoBehaviour
{
    public abstract void StartComponent(CharacterBase user);
    public abstract void OnHit(CharacterKnockback target);
    public abstract void EndComponent(CharacterBase user);
}