using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Combo : AttackComponent
{
    [SerializeField]
    [SuffixLabel("en frames")]
    private float timeCancel = 10f;
    [SerializeField] 
    private AttackManager comboAttack;

    float t = 0;

    public override void StartComponent(CharacterBase user)
    {
        t = 0f;
	}


    public override void UpdateComponent(CharacterBase user)
    {
        t += Time.deltaTime * user.MotionSpeed;
        if(t > timeCancel)
        {
            if(user.Input.CheckAction(0, InputConst.Special))
            {

            }
        }
    }

}
