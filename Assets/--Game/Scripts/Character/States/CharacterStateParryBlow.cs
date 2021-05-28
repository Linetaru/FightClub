using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateParryBlow : CharacterState
{
	[Title("Frame Data")]
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeBlow = 37;
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeStop = 6;

	[SerializeField]
	[SuffixLabel("en frames")]
	float timeCancelParry = 6;


	[Title("Ejection")]
	/*[SerializeField]
	float ejectionPower = 10f;*/
	[SerializeField]
	AnimationCurve ejectionCurve;

	[Title("Moveset")]
	[SerializeField]
	CharacterEvasiveMoveset evasiveMoveset;


	float t = 0f;
	float initialSpeedX = 0f;
	float initialSpeedY = 0f;


	private void Start()
	{
		timeBlow /= 60f;
		timeStop /= 60f;
		timeCancelParry /= 60f;
	}


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		evasiveMoveset.ResetDodge();
		t = timeBlow;


		initialSpeedX = character.Knockback.GetAngleKnockback().x * character.Movement.Direction;
		initialSpeedY = character.Knockback.GetAngleKnockback().y;

		character.Movement.SpeedX = initialSpeedX;
		character.Movement.SpeedY = initialSpeedY;

		Debug.Log(character.Movement.SpeedX);

		character.Rigidbody.PreventFall(false);
	}

	public override void UpdateState(CharacterBase character)
	{
		float coef = ejectionCurve.Evaluate(1 - ((t - (timeBlow - timeStop)) / timeStop));

		initialSpeedX = character.Knockback.GetAngleKnockback().x * character.Movement.Direction;
		initialSpeedY = character.Knockback.GetAngleKnockback().y;
		character.Movement.SpeedX = Mathf.Lerp(initialSpeedX, 0, coef); //character.Knockback.GetAngleKnockback().x * character.Movement.Direction;
		character.Movement.SpeedY = Mathf.Lerp(initialSpeedY, 0, coef); //character.Knockback.GetAngleKnockback().y;

		/*Debug.Log(initialSpeedX);
		Debug.Log(coef);
		Debug.Log(character.Movement.SpeedX);*/

		t -= Time.deltaTime * character.MotionSpeed;
		if (t <= (timeBlow - timeCancelParry))
		{
			evasiveMoveset.Parry(character);
			//evasiveMoveset.Dodge(character);
		}

		if (t <= 0)
			character.ResetToIdle();
	}
	
	public override void LateUpdateState(CharacterBase character)
	{
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		character.Rigidbody.PreventFall(true);
	}
}