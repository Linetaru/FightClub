using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterBase : MonoBehaviour, IControllable
{
	[SerializeField]
	CharacterState currentState;

	[SerializeField]
	CharacterState idleState;

	[SerializeField]
	CharacterState aerialState;
	public CharacterState CurrentState
	{
		get { return currentState; }
	}

	[Title("Model")]
	[SerializeField]
	private GameObject model;
	public GameObject Model
	{
		get { return model; }
	}

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
	private CharacterParticle particle;
	public CharacterParticle Particle
	{
		get { return particle; }
	}

	[SerializeField]
	private CharacterPowerGauge powerGauge;
	public CharacterPowerGauge PowerGauge
	{
		get { return powerGauge; }
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
		Movement.MotionSpeed = MotionSpeed;
		Knockback.MotionSpeed = MotionSpeed;
		action.InitializeComponent(this);
	}


	public void SetState(CharacterState characterState)
	{
		if(currentState != null)
			currentState.EndState(this, characterState);

		CharacterState oldState = currentState;
		currentState = characterState;

		currentState.StartState(this, oldState);

		OnStateChanged?.Invoke(oldState, currentState);
		//currentState = characterState;
	}


	// Update is called once per frame
	/*void Update()
	{
		currentState.UpdateState(this);
	}*/

	public void UpdateControl(int ID, Input_Info input_Info)
	{
		action.CanEndAction();

		input = input_Info;
		currentState.UpdateState(this);
		rigidbody.UpdateCollision(movement.SpeedX * movement.Direction * motionSpeed, movement.SpeedY * motionSpeed);
		currentState.LateUpdateState(this);
		powerGauge.ConsumePowerSegment(input_Info, this);

		action.EndActionState();
	}

	public void ResetToIdle()
    {
        if (rigidbody.IsGrounded)
        {
			SetState(idleState);
        }
        else
        {
			SetState(aerialState);
        }
    }

	public void SetMotionSpeed(float newValue, float time)
	{
		motionSpeed = newValue;
		Movement.MotionSpeed = MotionSpeed;
		Knockback.MotionSpeed = MotionSpeed;
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
		Knockback.MotionSpeed = MotionSpeed;
		Action.SetAttackMotionSpeed(MotionSpeed);
	}

}

