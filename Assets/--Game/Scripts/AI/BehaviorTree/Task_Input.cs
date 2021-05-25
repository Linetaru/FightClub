using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Task_Input : Action
{
    public SharedGameObject ai;
    public SharedCharacterBase user;
    public SharedCharacterBase target;

	DebugInput input;

    public bool pressButton = true;
    public EnumInput inputButton;
	public float horizontal = 0;
	public float vertical = 0;

    public bool inputTowardTarget = false;
	public bool inputWithDirection = false;

	AIBehaviorTree behaviorTree;

    public override void OnAwake()
    {
        behaviorTree = GetDefaultGameObject(ai.Value).GetComponent<AIBehaviorTree>();
        input = new DebugInput(0, null);
        input.horizontal = horizontal;
        input.vertical = vertical;
        if(pressButton)
            input.inputs.Add((int)inputButton);
    }



    public override TaskStatus OnUpdate()
    {
        if(inputWithDirection)
        {
            input.horizontal = input.horizontal * user.Value.Movement.Direction;
        }

        if (inputTowardTarget)
        {
            Vector2 direction = target.Value.transform.position - user.Value.transform.position;
            input.horizontal = direction.x;
            input.vertical = direction.x;
        }

        behaviorTree.AssignInput(input);

        return TaskStatus.Success;
	}

}
