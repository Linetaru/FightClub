using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class AIBehaviorTree : AIBehavior
{

	BehaviorTree behaviorTree = null;

	[Space]
	[SerializeField]
	public AIC_Target TargetsSystem;
	[SerializeField]
	public AIC_Pathfind PathfindSystem;
	[SerializeField]
	public List<AIC_Attacks> AttacksSystem;

	// Start is called before the first frame update
	void Start()
	{
		behaviorTree = GetComponent<BehaviorTree>();
		behaviorTree.SetVariableValue("AI", this.gameObject);
		inputs = new Input_Info();

	}

	// Update is called once per frame
	void Update()
	{
		if (isActive == false)
			return;

		this.transform.localScale = new Vector3(character.Movement.Direction, 1, 1);

		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActions);
		if (inputs.inputActions.Count != 0)
			inputController.UpdateTimeInBuffer(inputs.inputActionsUP);

		character.UpdateControl(0, inputs);
	}


	public override void StartBehavior()
	{
		base.StartBehavior();
		behaviorTree.enabled = true;

		behaviorTree.SetVariableValue("User", character);

		PathfindSystem.InitializeComponent(character, inputController, inputs);
		for (int i = 0; i < AttacksSystem.Count; i++)
		{
			AttacksSystem[i].InitializeComponent(character, inputController, inputs);
		}


		this.transform.SetParent(character.transform);
		this.transform.localPosition = Vector3.zero;


	}

	public override void StopBehavior()
	{
		base.StopBehavior();
		behaviorTree.enabled = false;
	}
}
