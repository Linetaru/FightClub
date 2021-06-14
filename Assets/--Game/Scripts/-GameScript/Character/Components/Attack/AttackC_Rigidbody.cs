using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_Rigidbody : AttackComponent
{
	[SerializeField]
	CharacterRigidbody characterRigidbody;


	//[SerializeField]
	//float lifetime = 120;
	[SerializeField]
	float baseSpeedX = 120;
	[SerializeField]
	AnimationCurve curveSpeed;
	[SerializeField]
	[SuffixLabel("en frames")]
	float timeCurve = 0.6f;

	[SerializeField]
	float baseSpeedY = 120;

	[InfoBox("1 = la direcition du joueur, -1 l'opposé, 0 osef")]
	[SerializeField]
	int baseDirection = 1;

	/*[Title("Deviate")]
	[SerializeField]
	Vector2 speedXDeviate = Vector2.zero;
	[SerializeField]
	Vector2 speedYDeviate = Vector2.zero;
	[SerializeField]
	Vector2 speedZDeviate = Vector2.zero;

	[Title("Repel")]
	[SerializeField]
	float speedMultiplier = 1.2f;*/

	[Title("Debug")]
	[SerializeField]
	Collider collider;

	[SerializeField]
	[SuffixLabel("en frames")]
	float startupFrame = 20f;

	//float life = 0;
	AttackSubManager attackSubManager;
	int direction;
	float speedX;
	float speedY;
	float t = 0;
	bool hitbox = false;
	//float speedZ;

	public override void StartComponent(CharacterBase user)
	{
		//collider.enabled = false;

		attackSubManager = this.transform.parent.GetComponent<AttackSubManager>();
		direction = baseDirection * user.Movement.Direction;
		speedX = baseSpeedX;
		speedY = baseSpeedY;
		t = 0f;
		timeCurve /= 60;
		startupFrame /= 60;
		hitbox = false;
	}

	public override void UpdateComponent(CharacterBase user)
	{
		characterRigidbody.UpdateCollision(speedX * direction * user.MotionSpeed, speedY * user.MotionSpeed);

		if (t > startupFrame && hitbox == false)
		{
			collider.enabled = true;
			hitbox = true;
		}

		if (t < timeCurve || t < startupFrame)
		{
			t += Time.deltaTime * user.MotionSpeed;
		}
		speedX = baseSpeedX * curveSpeed.Evaluate(t / timeCurve);
	}

	public override void EndComponent(CharacterBase user)
	{

	}

	public override void OnHit(CharacterBase user, CharacterBase target)
	{
		//Destroy(attackSubManager.gameObject);
	}

}
