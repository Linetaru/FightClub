using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionAtk : MonoBehaviour
{
    [SerializeField]
    AttackSubManager atkSub;

    [SerializeField]
    float timeExplosionActive = 0.1f;

    public void TriggerExplosion(CharacterBase character)
    {
        atkSub.GetComponentInChildren<AttackSubManager>();
        atkSub.InitAttack(character, "explosion");
        atkSub.ActionActive();

        StartCoroutine(ManageExplosion());
    }

    IEnumerator ManageExplosion()
    {
        yield return new WaitForSecondsRealtime(timeExplosionActive);
        atkSub.ActionUnactive();
    }
}
