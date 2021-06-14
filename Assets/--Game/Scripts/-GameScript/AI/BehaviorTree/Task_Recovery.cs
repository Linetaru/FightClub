using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Task_Recovery : Action
{
    public SharedGameObject ai;
    public SharedCharacterBase user;
    public float timeOut = 10;

    public float timeJump = 0.5f;
    public LayerMask layerMask;

    float t = 0f;

    AIBehaviorTree behaviorTree;
    AIC_Pathfind pathfind;


    Transform destination;
    DebugInput input;

    bool inDash = false;
    bool inRecovery = false;

    public override void OnAwake()
    {
        pathfind = GetDefaultGameObject(ai.Value).GetComponent<AIBehaviorTree>().PathfindSystem;
        behaviorTree = GetDefaultGameObject(ai.Value).GetComponent<AIBehaviorTree>();
    }

    public override void OnStart()
    {
        t = 0f;
        inRecovery = false;
        destination = pathfind.FindNearestNode(this.transform).transform;
        Debug.Log("IN RECOVERY");
    }

    public override TaskStatus OnUpdate()
    {
        if (t > 0)
        {
            t -= Time.deltaTime;
            return TaskStatus.Running;
        }


        if (user.Value.Movement.CurrentNumberOfJump != 0)
        {
            //Debug.Log("Je saute");
            input = new DebugInput(Mathf.Sign((destination.position - this.transform.position).x), 0, 0);
            behaviorTree.AssignInput(input);
            t = timeJump;
        }
        else
        {
            if (inRecovery == false)
            {
                input = new DebugInput(0, 1, 2); // UP B
                behaviorTree.AssignInput(input);
                inRecovery = true;
                t = 0.2f;
            }
            else
            {
                Vector2 direction = (destination.position - this.transform.position).normalized;
                input = new DebugInput(direction.x, direction.y);
                behaviorTree.AssignInput(input);
                t = timeJump*2;
            }
        }

  

        if (user.Value.Rigidbody.IsGrounded)
        {
            input = new DebugInput(0, 0);
            behaviorTree.AssignInput(input);
            return TaskStatus.Success;
        }
        else if (Physics.Raycast(this.transform.position, Vector3.down, 10f, layerMask))
        {
            input = new DebugInput(0, 0);
            behaviorTree.AssignInput(input);
            return TaskStatus.Success;
        }

        if (user.Value.Stats.Death)
            return TaskStatus.Failure;
        if (user.Value.Knockback.KnockbackDuration > 0)
            return TaskStatus.Failure;
        return TaskStatus.Running;
    }
}
