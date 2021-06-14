using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Task_CalculateTarget : Action
{
    public SharedGameObject user;
    public SharedCharacterBase target;

    AIC_Target targets;
    bool fail = false;

    public override void OnAwake()
    {
        targets = GetDefaultGameObject(user.Value).GetComponent<AIBehaviorTree>().TargetsSystem;
    }

    public override void OnStart()
    {
        base.OnStart();

        fail = false;
        CharacterBase t = targets.GetRandomTarget();

        if (t != null)
            target.SetValue(t);
        else
            fail = true;
    }

    public override TaskStatus OnUpdate()
    {
        if (fail == true)
            return TaskStatus.Failure;
        return TaskStatus.Success;
    }
}
