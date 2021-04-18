using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterParry : MonoBehaviour
{

	[Title("States")]
	[SerializeField]
	CharacterState parrySuccesState;
	public CharacterState ParrySuccesState
	{
		get { return parrySuccesState; }
	}

	[SerializeField]
	CharacterState parryRepelState;
	public CharacterState ParryRepelState
	{
		get { return parryRepelState; }
	}

	[Title("Parameter")]
	[SerializeField]
	private int[] timingParry;
	public int[] TimingParry
	{
		get { return timingParry; }
	}



	private int parryNumber;
	public int ParryNumber
	{
		get { return parryNumber; }
		set { parryNumber = value; }
	}

	private bool isParry;
	public bool IsParry
	{
		get { return isParry; }
		set { isParry = value; }
	}

	private bool isGuard;
	public bool IsGuard
	{
		get { return isGuard; }
		set { isGuard = value; }
	}

	CharacterBase characterParried;
	public CharacterBase CharacterParried
	{
		get { return characterParried; }
	}

	[Title("Particle - A virer plus tard")]
	[SerializeField]
	GameObject particleParry;
	public GameObject ParticleParry
	{
		get { return particleParry; }
	}

	[SerializeField]
	GameObject particleDirectionRepel;
	public GameObject ParticleDirectionRepel
	{
		get { return particleDirectionRepel; }
	}





	// Faire une interface ou une classe abstraire pour attackManager
	public virtual bool CanParry(AttackManager attackManager)
	{
		return isParry;
	}
	public virtual bool CanParry(AttackSubManager attackManager)
	{
		return isParry;
	}

	public virtual bool CanGuard(AttackSubManager attackManager)
	{
		return isGuard;
	}

	public virtual void Clash(CharacterBase user, AttackSubManager attack)
	{
		AttackSubManager attackUser = attack.AttackClashed;
		AttackSubManager attackEnemy = attack;
		if (attackUser.User == user) // C'est bien notre attaque
		{
			if(attackEnemy.ClashLevel > attackUser.ClashLevel) // User est repoussé
			{
				Parry(attackEnemy.User, user);
				attackEnemy.User.Knockback.Parry.ParryRepel(user, attackEnemy.User);
			}
			else if (attackUser.ClashLevel > attackEnemy.ClashLevel) // Enemy est repoussé
			{
				Parry(user, attackEnemy.User);
				attackEnemy.User.Knockback.Parry.ParryRepel(attackEnemy.User, user);
			}
			else // Match nul
			{
				user.SetMotionSpeed(0, 0.2f);
				attackEnemy.User.SetMotionSpeed(0f, 0.2f);

				Vector2 angleEjection = (user.transform.position - this.transform.position).normalized;
				GameObject go = Instantiate(particleParry, (user.Knockback.ContactPoint + attackEnemy.User.Knockback.ContactPoint) * 0.5f, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
				go.name = particleParry.name;
				Destroy(go, 1f);

				attackUser.ActionUnactive();
				attackEnemy.ActionUnactive();
			}

		}
	}




	/// <summary>
	/// Fonction à utiliser sur celui qui parry
	/// </summary>
	/// <param name="user"></param>
	public virtual void Parry(CharacterBase characterParry, CharacterBase characterRepelled)
	{
		isParry = false;
		characterParry.Knockback.ShakeEffect.Shake(0.05f, 0.1f);
		characterParry.SetMotionSpeed(0, 0.35f);
		characterParry.Action.CancelAction();
		characterParry.PowerGauge.ForceAddPower(20);



		characterParry.SetState(parrySuccesState);
		characterParry.Action.HasHit(characterRepelled);

		characterParried = characterRepelled;

		Vector2 angleEjection = (characterRepelled.transform.position - characterParry.transform.position).normalized;
		GameObject go = Instantiate(particleParry, characterParry.Knockback.ContactPoint, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
		go.name = particleParry.name;
		Destroy(go, 1f);
	}

	/// <summary>
	/// Fonction à utiliser sur celui qui se fait parry
	/// </summary>
	/// <param name="user"></param>
	public virtual void ParryRepel(CharacterBase characterRepelled, CharacterBase characterParry)
	{
		isParry = false;
		characterRepelled.Knockback.ShakeEffect.Shake(0.12f, 0.3f);
		characterRepelled.SetMotionSpeed(0f, 0.35f);
		characterRepelled.Action.CancelAction();

		characterRepelled.Model.FlashModel(Color.white, 0.7f);


		Vector2 angleEjection = (characterRepelled.transform.position - characterParry.transform.position).normalized;
		characterRepelled.Knockback.Launch(angleEjection, 1);

		characterRepelled.SetState(parryRepelState);


		GameObject go = Instantiate(particleDirectionRepel, characterParry.Knockback.ContactPoint, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
		go.name = particleParry.name;
		Destroy(go, 1f);
	}

}
