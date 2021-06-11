using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Task_Attacks : Action
{
    public SharedGameObject ai;
    public SharedCharacterBase target;

    public int indexAttack = 0;
    public float timeOut = 5;

    AIC_Attacks attack;


    public override void OnAwake()
    {
        attack = GetDefaultGameObject(ai.Value).GetComponent<AIBehaviorTree>().AttacksSystem[indexAttack];
    }

    public override void OnStart()
    {
        attack.CurrentAttack.StartAttack(target.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (attack.CurrentAttack.UpdateAttack(target.Value) == false)
        {
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Running;
    }
}
