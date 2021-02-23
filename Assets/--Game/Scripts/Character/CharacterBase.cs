using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterBase : MonoBehaviour, IControllable
{
	[SerializeField]
	CharacterState currentState;

	[Title("Components")]
	[SerializeField]
	private CharacterRigidbody rigidbody;
	public CharacterRigidbody Rigidbody
	{
		get { return rigidbody; }
	}

	[SerializeField]
	private CharacterMovement movement;
	public CharacterMovement Movement
	{
		get { return movement; }
	}

	[SerializeField]
	private CharacterAction action;
	public CharacterAction Action
	{
		get { return action; }
	}

	[SerializeField]
	private CharacterKnockback knockback;
	public CharacterKnockback Knockback
	{
		get { return knockback; }
	}

	[SerializeField]
	private CharacterStats stats;
	public CharacterStats Stats
	{
		get { return stats; }
	}

	[SerializeField]
	private CharacterUI ui;
	public CharacterUI Ui
	{
		get { return ui; }
	}

	private Input_Info input;
	public Input_Info Input
	{
		get { return input; }
	}

	public delegate void ActionSetState(CharacterState oldState, CharacterState newState);
	public event ActionSetState OnStateChanged;
	public delegate void ActionFloat(float value);
	public event ActionFloat OnMotionSpeed;

	[Title("Parameter")]
	[SerializeField]
	[ReadOnly]
	private float motionSpeed = 1;
	public float MotionSpeed
	{
		get { return motionSpeed; }
	}
	private IEnumerator motionSpeedCoroutine;


	// Start is called before the first frame update
	void Start()
	{
		Application.targetFrameRate = 60;
		movement.MotionSpeed = MotionSpeed;
		action.InitializeComponent(this);
		Stats.InitStats();
		Ui.InitPlayerPanel(this);
	}


	public void SetState(CharacterState characterState)
	{
		if(currentState != null)
			currentState.EndState(this, characterState);
		characterState.StartState(this, currentState);

		OnStateChanged?.Invoke(currentState, characterState);
		currentState = characterState;
	}


	// Update is called once per frame
	/*void Update()
	{
		currentState.UpdateState(this);
	}*/

	public void UpdateControl(int ID, Input_Info input_Info)
	{
		input = input_Info;
		currentState.UpdateState(this);
		rigidbody.UpdateCollision(movement.SpeedX * movement.Direction * motionSpeed, movement.SpeedY * motionSpeed);
		currentState.LateUpdateState(this);
	}



	public void SetMotionSpeed(float newValue, float time)
	{
		motionSpeed = newValue;
		Movement.MotionSpeed = MotionSpeed;
		Action.SetAttackMotionSpeed(MotionSpeed);

		if (motionSpeedCoroutine != null)
			StopCoroutine(motionSpeedCoroutine);
		motionSpeedCoroutine = MotionSpeedCoroutine(time);
		StartCoroutine(motionSpeedCoroutine);
	}

	private IEnumerator MotionSpeedCoroutine(float time)
	{
		while (time > 0)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		motionSpeed = 1;
		Movement.MotionSpeed = MotionSpeed;
		Action.SetAttackMotionSpeed(MotionSpeed);
	}

}

