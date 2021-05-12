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

	[SerializeField]
	private float parryAngle;
	public float ParryAngle
	{
		get { return parryAngle; }
		set { parryAngle = value; } // Pour les tests, ne pas mettre en write sinon
	}



	private Vector2 parryDirection;
	public Vector2 ParryDirection
	{
		get { return parryDirection; }
		set { parryDirection = value; }
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

	private bool isGuardDash;
	public bool IsGuardDash
	{
		get { return isGuardDash; }
		set { isGuardDash = value; }
	}


	CharacterBase characterParried;
	public CharacterBase CharacterParried
	{
		get { return characterParried; }
	}





	public event EventCharacterBase OnParry;
	public event EventCharacterBase OnGuard;
	public event EventAttackSubManager OnClash;




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

	[SerializeField]
	GameObject particleGuard;
	public GameObject ParticleGuard
	{
		get { return particleGuard; }
	}
	[SerializeField]
	GameObject particleGuardDirectionRepel;
	public GameObject ParticleGuardDirectionRepel
	{
		get { return particleGuardDirectionRepel; }
	}


	// Faire une interface ou une classe abstraire pour attackManager
	public virtual bool CanParry(AttackManager attackManager)
	{
		return isParry;
	}
	/*public virtual bool CanParry(AttackSubManager attackManager)
	{
		return isParry;
	}*/

	// Directionnal
	public virtual bool CanParry(AttackSubManager attackManager)
	{
		if (isParry == false)
			return false;
		else if (attackManager.BreakParry == true)
			return false;
		return CheckAngle(attackManager);
	}

	/*public virtual bool CanGuard(AttackSubManager attackManager)
	{
		return isGuard;
	}*/

	public virtual bool CanGuard(AttackSubManager attackManager)
	{
		if (isGuardDash == true && attackManager.GuardOnDash == true)
			return true;
		if (isGuard == false)
			return false;
		else if (attackManager.BreakGuard == true)
			return false;
		return CheckAngle(attackManager);
	}

	public bool CheckAngle(AttackSubManager attackManager)
	{
		float angle = Vector2.Angle((attackManager.transform.position - this.transform.position), parryDirection);
		//return (parryDirection - (parryAngle *0.5f) < angle && angle < parryDirection + (parryAngle * 0.5f));
		return angle < (parryAngle * 0.5f);
	}

	public virtual void Clash(CharacterBase user, AttackSubManager attack)
	{
		AttackSubManager attackUser = attack.AttackClashed;
		AttackSubManager attackEnemy = attack;
		if (attackUser.User == user) // C'est bien notre attaque
		{
			if(attackEnemy.ClashLevel > attackUser.ClashLevel) // User est repoussé
			{
				attackEnemy.User.Knockback.Parry.ParryResolution(attackEnemy.User, attackUser);
				/*attackEnemy.User.Knockback.Parry.Parry(attackEnemy.User, user);
				ParryRepel(user, attackEnemy.User);*/
			}
			else if (attackUser.ClashLevel > attackEnemy.ClashLevel) // Enemy est repoussé
			{
				ParryResolution(user, attackEnemy);
				/*Parry(user, attackEnemy.User);
				attackEnemy.User.Knockback.Parry.ParryRepel(attackEnemy.User, user);*/
			}
			else // Match nul
			{
				if(attackUser.Disjoint == false)
					user.SetMotionSpeed(0, 0.2f);

				if(attackEnemy.Disjoint == false)
					attackEnemy.User.SetMotionSpeed(0f, 0.2f);

				Vector2 angleEjection = (user.transform.position - this.transform.position).normalized;
				GameObject go = Instantiate(particleParry, (user.Knockback.ContactPoint + attackEnemy.User.Knockback.ContactPoint) * 0.5f, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
				go.name = particleParry.name;
				Destroy(go, 1f);

				attackUser.ActionUnactive();
				attackEnemy.ActionUnactive();

				attackUser.Clash(attackEnemy.User);
				attackEnemy.Clash(attackUser.User);

				OnClash?.Invoke(attackEnemy);

			}

		}
	}


	public virtual void ParryResolution(CharacterBase character, AttackSubManager atkRegistered)
	{
		atkRegistered.AddPlayerHitList(character.tag);

		Parry(character, atkRegistered.User);
		if (atkRegistered.Disjoint == false)
		{
			atkRegistered.User.Knockback.Parry.ParryRepel(atkRegistered.User, character);
		}


		// Pour tourner le joueur dans le sens de la parry
		if (Mathf.Sign(atkRegistered.User.transform.position.x - character.transform.position.x) != character.Movement.Direction)
			character.Movement.Direction *= -1;

		// Feedback
		Vector2 angleEjection = (atkRegistered.User.transform.position - character.transform.position).normalized;
		GameObject go2 = Instantiate(particleParry, character.Knockback.ContactPoint, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
		go2.name = particleParry.name;
		Destroy(go2, 1f);

		GameObject go = Instantiate(particleDirectionRepel, character.Knockback.ContactPoint, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
		go.name = particleParry.name;
		Destroy(go, 1f);

		atkRegistered.Parry(character);
	}

	public virtual void GuardResolution(CharacterBase character, AttackSubManager atkRegistered)
	{
		atkRegistered.AddPlayerHitList(character.tag);
		//	atkRegistered.User.Knockback.ContactPoint = character.Knockback.ContactPoint;

		if (atkRegistered.GuardWin == false)
		{
			Guard(character, atkRegistered.User);
		}
		if (atkRegistered.Disjoint == false)
		{
			atkRegistered.User.Knockback.Parry.Parry(atkRegistered.User, character);
			atkRegistered.User.PowerGauge.ForceAddPower(-25);
		}

		// Pour tourner le joueur dans le sens de la garde
		if (Mathf.Sign(atkRegistered.User.transform.position.x - character.transform.position.x) != character.Movement.Direction)
			character.Movement.Direction *= -1;

		// Feedback
		if (atkRegistered.GuardWin == true)
		{
			character.Knockback.ShakeEffect.Shake(0.1f, 0.3f);
			character.Model.FlashModel(Color.white, 0.5f);

			Vector2 angleEjection = (character.transform.position - atkRegistered.User.transform.position).normalized;
			GameObject go = Instantiate(particleParry, character.CenterPoint.position, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
			go.name = particleParry.name;
			Destroy(go, 1f);
		}
		else
		{
			Vector2 angleEjection = (character.transform.position - atkRegistered.User.transform.position).normalized;
			GameObject go2 = Instantiate(particleGuard, character.CenterPoint.position, Quaternion.identity);
			go2.name = particleParry.name;
			Destroy(go2, 1f);

			GameObject go = Instantiate(particleGuardDirectionRepel, character.CenterPoint.position, Quaternion.Euler(0, 0, -Mathf.Atan2(angleEjection.x, angleEjection.y) * Mathf.Rad2Deg));
			go.name = particleParry.name;
			Destroy(go, 1f);
		}

		atkRegistered.Guard(character);
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

		characterParry.PowerGauge.ForceAddPower(25);



		characterParry.SetState(parrySuccesState);
		characterParry.Action.HasHit(characterRepelled);

		characterParried = characterRepelled;

		Vector2 angleEjection = (characterRepelled.transform.position - characterParry.transform.position).normalized;

		OnParry?.Invoke(characterParried);
		Feedbacks.GlobalFeedback.Instance.CameraRotationImpulse(new Vector2(-angleEjection.y, angleEjection.x) * 4, 0.15f);
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

	}


	public virtual void Guard(CharacterBase characterRepelled, CharacterBase characterParry)
	{
		isParry = false;
		characterRepelled.Knockback.ShakeEffect.Shake(0.12f, 0.5f);
		characterRepelled.SetMotionSpeed(0f, 0.35f);
		characterRepelled.Action.CancelAction();

		characterRepelled.Model.FlashModel(Color.white, 0.7f);


		Vector2 angleEjection = (characterRepelled.transform.position - characterParry.transform.position).normalized;
		characterRepelled.Knockback.Launch(angleEjection, 1);

		characterRepelled.SetState(parryRepelState);
		OnGuard?.Invoke(characterParry);

	}

}
