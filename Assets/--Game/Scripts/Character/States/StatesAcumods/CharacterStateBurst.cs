using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateBurst : CharacterState
{
	[SerializeField]
	float timeBurst = 20;


	[Title("Feedbacks")]
	[SerializeField]
	ParticleSystem burstParticle;

	[SerializeField]
	AttackManager burstAttack;

	AttackManager attack;
	float t = 0;

	private void Start()
	{
		timeBurst /= 60f;
	}


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		t = 0f;
		ParticleSystem particle = Instantiate(burstParticle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
		Destroy(particle.gameObject, 0.5f);

		attack = Instantiate(burstAttack, character.CenterPoint.position, Quaternion.identity);
		attack.CreateAttack(character);
		attack.ActionActive();

		character.Movement.SetSpeed(0, 0);

	}

	public override void UpdateState(CharacterBase character)
	{
		attack.ActionUnactive();
		t += Time.deltaTime;
		if (t >= timeBurst)
			character.ResetToIdle();
	}
	
	public override void LateUpdateState(CharacterBase character)
	{
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{
		attack.EndAction();
	}
}