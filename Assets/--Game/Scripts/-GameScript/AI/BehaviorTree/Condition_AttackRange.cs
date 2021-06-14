using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Condition_AttackRange : Conditional
{
    public SharedGameObject ai;
    public SharedCharacterBase target;

    public int indexAttack = 0;
    AIC_Attacks attack;

    public override void OnAwake()
    {
        attack = GetDefaultGameObject(ai.Value).GetComponent<AIBehaviorTree>().AttacksSystem[indexAttack];
    }



    public override TaskStatus OnUpdate()
    {
        return attack.CheckAttacks(target.Value) ? TaskStatus.Success : TaskStatus.Failure;

    }

}