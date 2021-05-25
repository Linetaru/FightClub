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


	//float life = 0;
	AttackSubManager attackSubManager;
	int direction;
	float speedX;
	float speedY;
	//float speedZ;

	public override void StartComponent(CharacterBase user)
	{
		collider.enabled = true;

		attackSubManager = this.transform.parent.GetComponent<AttackSubManager>();
		direction = baseDirection * user.Movement.Direction;
		speedX = baseSpeedX;
		speedY = baseSpeedY;
		//speedZ = 0;
	}

	public override void UpdateComponent(CharacterBase user)
	{
		characterRigidbody.UpdateCollision(speedX * direction * user.MotionSpeed, speedY * user.MotionSpeed);

		/*attackSubManager.transform.position += new Vector3(speed * direction, speedY, speedZ) * Time.deltaTime;

		life += Time.deltaTime;
		if (life >= lifetime)
			Destroy(attackSubManager.gameObject);*/
	}

	public override void EndComponent(CharacterBase user)
	{

	}

	public override void OnHit(CharacterBase user, CharacterBase target)
	{
		//Destroy(attackSubManager.gameObject);
	}

	public override void OnParry(CharacterBase user, CharacterBase target)
	{
		/*Debug.Log(user.tag);
		Debug.Log(target.tag);
		attackSubManager.ReInitAttack(target, attackSubManager.gameObject.name);
		direction *= -1;
		speed *= speedMultiplier;
		life = 0;
		collider.enabled = true;*/

	}
	public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
	{
		/*if(guardRepel == false)
		{
			speed = Random.Range(speedXDeviate.x, speedXDeviate.y);
			speed *= -1;
			speedY = Random.Range(speedYDeviate.x, speedYDeviate.y);
			speedZ = Random.Range(speedZDeviate.x, speedZDeviate.y);
			if (Random.Range(0, 2) == 0)
				speedZ *= -1;
		}*/
	}
	public override void OnClash(CharacterBase user, CharacterBase target)
	{
		//Destroy(attackSubManager.gameObject);
	}
}
