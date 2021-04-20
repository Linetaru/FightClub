using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BallBase : MonoBehaviour
{
	[SerializeField]
	CharacterState currentState;

	[SerializeField]
	CharacterState idleState;

	[SerializeField]
	CharacterState knockbackState;

	[SerializeField]
	CharacterState buntState;

	[SerializeField]
	CharacterState landingState;

	public CharacterState CurrentState
	{
		get { return currentState; }
	}

	//[Title("Model")]
	//[SerializeField]
	//private Transform centerPoint;
	//public Transform CenterPoint
	//{
	//	get { return centerPoint; }
	//}

	//[SerializeField]
	//private CharacterModel model;
	//public CharacterModel Model
	//{
	//	get { return model; }
	//}

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
	private CharacterStatusEffects status;
	public CharacterStatusEffects Status
	{
		get { return status; }
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

	[SerializeField]
	private CharacterProjectile projectile;
	public CharacterProjectile Projectile
	{
		get { return projectile; }
	}

	private Input_Info input;
	public Input_Info Input
	{
		get { return input; }
	}


	private int playerID;
	public int PlayerID
	{
		get { return playerID; }
		set { playerID = value; }
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
		status.InitializeComponent(this);
	}


	public void SetState(CharacterState characterState)
	{
		//Debug.Log(characterState.gameObject.name);
		if (currentState != null)
			currentState.EndState(this, characterState);

		CharacterState oldState = currentState;
		currentState = characterState;

		currentState.StartState(this, oldState);

		OnStateChanged?.Invoke(oldState, currentState);
	}




	public void UpdateControl(int ID, Input_Info input_Info)
	{
		// Les animation event se lancent avant l'update
		// Action.CanEndAction() se lance en tout début pour bien recevoir les animation event
		action.CanEndAction();

		// Les OnTrigger se lancent avant l'update
		// Knockback.CheckHit se lance en tout début pour bien recevoir les collisions
		knockback.CheckHit(this);

		input = input_Info;
		status.UpdateStatus();
		currentState.UpdateState(this);
		rigidbody.UpdateCollision(movement.SpeedX * movement.Direction * motionSpeed, movement.SpeedY * motionSpeed);
		currentState.LateUpdateState(this);
		powerGauge.ConsumePowerSegment(input_Info, this);

		action.EndActionState();
	}


	// Aveux de faiblesse pardon les amis
	public void ResetToIdle()
	{
		if (rigidbody.IsGrounded)
		{
			SetState(idleState);
		}
		else
		{
			ResetToAerial();
		}
	}

	public void ResetToAerial()
	{
		SetState(aerialState);
	}

	public void ResetToLand()
	{
		SetState(landingState);
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
