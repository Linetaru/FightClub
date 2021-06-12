using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Task_Pathfinding : Action
{
    public SharedGameObject user;
    public SharedCharacterBase target;
    public float timeOut = 10;

    float t = 0f;

    AIC_Pathfind pathfind;

    public override void OnAwake()
    {
        pathfind = GetDefaultGameObject(user.Value).GetComponent<AIBehaviorTree>().PathfindSystem;
    }

    public override void OnStart()
    {
        t = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        t += Time.deltaTime;
        if (t > timeOut)
        {
            Debug.Log("timeOut");
            return TaskStatus.Failure;
        }

        pathfind.UpdatePath(target.Value.transform);
        return TaskStatus.Running;
    }
}
